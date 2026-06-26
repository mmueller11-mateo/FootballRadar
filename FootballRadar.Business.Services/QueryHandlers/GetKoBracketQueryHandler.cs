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

        public async Task<KoBracketResult> Handle(GetKoBracketQuery request, CancellationToken cancellationToken)
        {
            var allFixtures = await fixtureRepository.GetAllAsync(cancellationToken);
            var allTeams = await nationalTeamRepository.GetAllAsync(cancellationToken);
            var userTips = await wmTipRepository.GetByUserIdAsync(request.UserId, cancellationToken);

            var koFixtures = allFixtures
                .Where(f => f.WmPhase.HasValue && f.WmPhase != WmPhase.Group)
                .OrderBy(f => f.Date)
                .ToList();

            var koTips = userTips.Where(t => t.IsKoMatch).ToList();

            return new KoBracketResult
            {
                RoundOf32 = BuildRound(koFixtures, WmPhase.RoundOf32, allTeams, koTips),
                RoundOf16 = BuildRound(koFixtures, WmPhase.RoundOf16, allTeams, koTips),
                QuarterFinals = BuildRound(koFixtures, WmPhase.QuarterFinal, allTeams, koTips),
                SemiFinals = BuildRound(koFixtures, WmPhase.SemiFinal, allTeams, koTips),
                ThirdPlace = BuildRound(koFixtures, WmPhase.ThirdPlace, allTeams, koTips).FirstOrDefault(),
                Final = BuildRound(koFixtures, WmPhase.Final, allTeams, koTips).FirstOrDefault(),
            };
        }

        private static List<KoMatchResult> BuildRound(
            List<Match> fixtures,
            WmPhase phase,
            IEnumerable<NationalTeam> allTeams,
            List<WmTip> koTips)
        {
            return fixtures
                .Where(f => f.WmPhase == phase)
                .OrderBy(f => f.Date)
                .Select(f =>
                {
                    var home = allTeams.FirstOrDefault(t => t.Id == f.HomeNationalTeamId);
                    var away = allTeams.FirstOrDefault(t => t.Id == f.AwayNationalTeamId);
                    var tip = koTips.FirstOrDefault(t => t.WmMatchId == f.Id);

                    return new KoMatchResult
                    {
                        FixtureId = f.Id,
                        ApiFixtureId = f.ApiFixtureId,
                        KickoffUtc = f.Date,
                        HomeTeam = home,
                        AwayTeam = away,
                        HomeQualificationCode = f.HomeQualificationCode,
                        AwayQualificationCode = f.AwayQualificationCode,
                        ExistingTip = tip,
                    };
                })
                .ToList();
        }
    }
}