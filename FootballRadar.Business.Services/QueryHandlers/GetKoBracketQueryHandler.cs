using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Enums;
using FootballRadar.Business.Entities.LeagueEntities;
using FootballRadar.Business.Entities.TeamEntities;
using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.Business.Services.Queries;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    public class GetKoBracketQueryHandler : IRequestHandler<GetKoBracketQuery, KoBracketResult>
    {
        private readonly IMatchRepository fixtureRepository;
        private readonly INationalTeamRepository nationalTeamRepository;
        private readonly IWmTipRepository wmTipRepository;
        private readonly WmBracketResolverService resolver;

        public GetKoBracketQueryHandler(
            IMatchRepository fixtureRepository,
            INationalTeamRepository nationalTeamRepository,
            IWmTipRepository wmTipRepository,
            WmBracketResolverService resolver)
        {
            this.fixtureRepository = fixtureRepository;
            this.nationalTeamRepository = nationalTeamRepository;
            this.wmTipRepository = wmTipRepository;
            this.resolver = resolver;
        }

        public async Task<KoBracketResult> Handle(
            GetKoBracketQuery request,
            CancellationToken cancellationToken)
        {
            var allFixtures = await fixtureRepository.GetAllAsync(cancellationToken);
            var allTeams = await nationalTeamRepository.GetAllAsync(cancellationToken);
            var userTips = await wmTipRepository.GetByUserIdAsync(request.UserId, cancellationToken);

            var groupFixtures = allFixtures
                .Where(f => f.WmPhase == WmPhase.Group)
                .ToList();

            var groupStandings = BuildGroupStandings(groupFixtures, userTips, allTeams);

            var koFixtures = allFixtures
                .Where(f => f.WmPhase.HasValue && f.WmPhase != WmPhase.Group)
                .OrderBy(f => f.Date)
                .ToList();

            // Teams pro ApiFixtureId auflösen
            var resolvedTeams = new Dictionary<int, (NationalTeam? Home, NationalTeam? Away)>();

            // Verhindert doppelte Drittplatzierte
            var usedTeamIds = new HashSet<Guid>();

            foreach (var fixture in koFixtures)
            {
                var home = ResolveSlot(
                    fixture.HomeQualificationCode,
                    groupStandings,
                    allTeams,
                    userTips,
                    koFixtures,
                    resolvedTeams,
                    usedTeamIds);

                var away = ResolveSlot(
                    fixture.AwayQualificationCode,
                    groupStandings,
                    allTeams,
                    userTips,
                    koFixtures,
                    resolvedTeams,
                    usedTeamIds);

                resolvedTeams[fixture.ApiFixtureId] = (home, away);
            }

            var koTips = userTips.Where(t => t.IsKoMatch).ToList();

            return new KoBracketResult
            {
                RoundOf32 = BuildRound(koFixtures, WmPhase.RoundOf32, resolvedTeams, koTips),
                RoundOf16 = BuildRound(koFixtures, WmPhase.RoundOf16, resolvedTeams, koTips),
                QuarterFinals = BuildRound(koFixtures, WmPhase.QuarterFinal, resolvedTeams, koTips),
                SemiFinals = BuildRound(koFixtures, WmPhase.SemiFinal, resolvedTeams, koTips),
                ThirdPlace = BuildRound(koFixtures, WmPhase.ThirdPlace, resolvedTeams, koTips)
                    .FirstOrDefault(),
                Final = BuildRound(koFixtures, WmPhase.Final, resolvedTeams, koTips)
                    .FirstOrDefault(),
            };
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // SLOT RESOLVER
        // ─────────────────────────────────────────────────────────────────────────────

        private NationalTeam? ResolveSlot(
            string? code,
            IReadOnlyDictionary<string, List<GroupStandingEntry>> groupStandings,
            IEnumerable<NationalTeam> allTeams,
            IEnumerable<WmTip> userTips,
            List<Match> koFixtures,
            Dictionary<int, (NationalTeam? Home, NationalTeam? Away)> resolvedTeams,
            HashSet<Guid> usedTeamIds)
        {
            if (string.IsNullOrEmpty(code))
                return null;

            // Gewinner eines KO-Spiels
            if (code.StartsWith("W") && int.TryParse(code[1..], out var winnerOf))
            {
                var source = koFixtures.FirstOrDefault(f => f.ApiFixtureId == winnerOf);

                if (source == null)
                    return null;

                var tip = userTips.FirstOrDefault(t =>
                    t.WmMatchId == source.Id &&
                    t.IsKoMatch);

                if (tip?.PredictedWinnerId == null)
                    return null;

                return allTeams.FirstOrDefault(t => t.Id == tip.PredictedWinnerId);
            }

            // Verlierer eines KO-Spiels
            if (code.StartsWith("L") && int.TryParse(code[1..], out var loserOf))
            {
                var source = koFixtures.FirstOrDefault(f => f.ApiFixtureId == loserOf);

                if (source == null)
                    return null;

                var tip = userTips.FirstOrDefault(t =>
                    t.WmMatchId == source.Id &&
                    t.IsKoMatch);

                if (tip?.PredictedWinnerId == null)
                    return null;

                if (!resolvedTeams.TryGetValue(loserOf, out var pair))
                    return null;

                if (pair.Home == null || pair.Away == null)
                    return null;

                return tip.PredictedWinnerId == pair.Home.Id
                    ? pair.Away
                    : pair.Home;
            }

            // Gruppenplatzierungen / Drittplatzierte
            return resolver.Resolve(
                code,
                groupStandings,
                allTeams,
                usedTeamIds);
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // ROUND BUILDER
        // ─────────────────────────────────────────────────────────────────────────────

        private static List<KoMatchResult> BuildRound(
            List<Match> fixtures,
            WmPhase phase,
            Dictionary<int, (NationalTeam? Home, NationalTeam? Away)> resolvedTeams,
            List<WmTip> koTips)
        {
            return fixtures
                .Where(f => f.WmPhase == phase)
                .OrderBy(f => f.Date)
                .Select(f =>
                {
                    resolvedTeams.TryGetValue(f.ApiFixtureId, out var pair);

                    var tip = koTips.FirstOrDefault(t => t.WmMatchId == f.Id);

                    return new KoMatchResult
                    {
                        FixtureId = f.Id,
                        ApiFixtureId = f.ApiFixtureId,
                        KickoffUtc = f.Date,

                        HomeTeam = pair.Home,
                        AwayTeam = pair.Away,

                        HomeQualificationCode = f.HomeQualificationCode,
                        AwayQualificationCode = f.AwayQualificationCode,

                        ExistingTip = tip,
                    };
                })
                .ToList();
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // GROUP STANDINGS
        // ─────────────────────────────────────────────────────────────────────────────

        private IReadOnlyDictionary<string, List<GroupStandingEntry>> BuildGroupStandings(
            List<Match> groupFixtures,
            IEnumerable<WmTip> userTips,
            IEnumerable<NationalTeam> allTeams)
        {
            var tipsList = userTips.ToList();
            var teamsList = allTeams.ToList();

            var result = new Dictionary<string, List<GroupStandingEntry>>();

            foreach (var g in groupFixtures
                         .Where(f => f.WmGroup != null)
                         .GroupBy(f => f.WmGroup!))
            {
                var stats = new Dictionary<Guid, GroupStandingEntry>();

                void Ensure(Guid teamId)
                {
                    if (!stats.ContainsKey(teamId))
                    {
                        var team = teamsList.FirstOrDefault(t => t.Id == teamId);

                        stats[teamId] = new GroupStandingEntry
                        {
                            TeamId = teamId,
                            TeamName = team?.Name ?? "",
                        };
                    }
                }

                foreach (var fixture in g)
                {
                    if (fixture.HomeNationalTeamId == null ||
                        fixture.AwayNationalTeamId == null)
                        continue;

                    var homeId = fixture.HomeNationalTeamId.Value;
                    var awayId = fixture.AwayNationalTeamId.Value;

                    Ensure(homeId);
                    Ensure(awayId);

                    var tip = tipsList.FirstOrDefault(t => t.WmMatchId == fixture.Id);

                    int homeGoals;
                    int awayGoals;

                    if (tip != null)
                    {
                        homeGoals = tip.HomeGoals;
                        awayGoals = tip.AwayGoals;
                    }
                    else if (fixture.HomeGoals.HasValue &&
                             fixture.AwayGoals.HasValue)
                    {
                        homeGoals = fixture.HomeGoals.Value;
                        awayGoals = fixture.AwayGoals.Value;
                    }
                    else
                    {
                        continue;
                    }

                    var home = stats[homeId];
                    var away = stats[awayId];

                    home.Played++;
                    away.Played++;

                    home.GoalsFor += homeGoals;
                    home.GoalsAgainst += awayGoals;

                    away.GoalsFor += awayGoals;
                    away.GoalsAgainst += homeGoals;

                    if (homeGoals > awayGoals)
                    {
                        home.Won++;
                        away.Lost++;
                    }
                    else if (homeGoals < awayGoals)
                    {
                        home.Lost++;
                        away.Won++;
                    }
                    else
                    {
                        home.Drawn++;
                        away.Drawn++;
                    }
                }

                result[g.Key] = stats.Values
                    .OrderByDescending(t => t.Points)
                    .ThenByDescending(t => t.GoalDifference)
                    .ThenByDescending(t => t.GoalsFor)
                    .ToList();
            }

            return result;
        }
    }
}