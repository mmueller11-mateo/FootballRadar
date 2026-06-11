using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting.Enums;
using FootballRadar.Business.Entities.Enums;
using FootballRadar.Business.Services.Queries;
using FootballRadar.Business.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Data.Repositories
{
    internal sealed class ViewModelRepository : IViewModelRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> dbContextFactory;
        private readonly IMediator mediator;
        private readonly INationalTeamRepository nationalTeamRepository;
        private readonly IUserRepository userRepository;
        private readonly ILeagueRepository leagueRepository;

        public ViewModelRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory, IMediator mediator, INationalTeamRepository nationalTeamRepository, IUserRepository userRepository, ILeagueRepository leagueRepository)
        {
            this.dbContextFactory = dbContextFactory;
            this.mediator = mediator;
            this.nationalTeamRepository = nationalTeamRepository;
            this.userRepository = userRepository;
            this.leagueRepository = leagueRepository;
        }

        public async Task<PlaceBetViewModel> CreatePlaceBetViewModel(Guid matchId, Guid userId, CancellationToken cancellationToken)
        {
            await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var matchData = await (
                from match in db.Fixtures.AsNoTracking()
                join homeTeamJoin in db.Teams on match.HomeTeamId equals homeTeamJoin.Id into homeJoin
                from homeTeam in homeJoin.DefaultIfEmpty()
                join awayTeamJoin in db.Teams on match.AwayTeamId equals awayTeamJoin.Id into awayJoin
                from awayTeam in awayJoin.DefaultIfEmpty()
                where match.Id == matchId
                select new
                {
                    MatchId = match.Id,
                    HomeTeamId = match.HomeTeamId,
                    AwayTeamId = match.AwayTeamId,
                    HomeTeamName = homeTeam != null ? homeTeam.Name : (match.HomeQualificationCode ?? string.Empty),
                    AwayTeamName = awayTeam != null ? awayTeam.Name : (match.AwayQualificationCode ?? string.Empty),
                    HomeLogo = homeTeam != null ? homeTeam.Logo : null,
                    AwayLogo = awayTeam != null ? awayTeam.Logo : null,
                    MatchDate = match.Date,
                    Round = match.Round,
                    Credits = db.Wallets
                        .Where(w => w.UserId == userId)
                        .Select(w => w.Credits)
                        .FirstOrDefault(),
                    IsKnockout = match.WmPhase != null && match.WmPhase != WmPhase.Group
                }
            ).FirstOrDefaultAsync(cancellationToken);

            if (matchData is null)
                throw new Exception("Match not found");

            var h2h = await (
                from m in db.Fixtures.AsNoTracking()
                join hht in db.Teams on m.HomeTeamId equals hht.Id
                join hat in db.Teams on m.AwayTeamId equals hat.Id
                where
                    (m.HomeTeamId == matchData.HomeTeamId && m.AwayTeamId == matchData.AwayTeamId) ||
                    (m.HomeTeamId == matchData.AwayTeamId && m.AwayTeamId == matchData.HomeTeamId)
                where m.HomeGoals.HasValue && m.AwayGoals.HasValue
                orderby m.Date descending
                select new HeadToHeadViewModel
                {
                    Date = m.Date,
                    HomeTeam = hht.Name,
                    AwayTeam = hat.Name,
                    HomeGoals = m.HomeGoals!.Value,
                    AwayGoals = m.AwayGoals!.Value
                }
            ).Take(5).ToListAsync(cancellationToken);

            return new PlaceBetViewModel
            {
                MatchId = matchData.MatchId,
                Credits = 0,
                Prediction = MatchPrediction.HomeWin,
                HomeTeam = matchData.HomeTeamName,
                AwayTeam = matchData.AwayTeamName,
                HomeLogo = matchData.HomeLogo,
                AwayLogo = matchData.AwayLogo,
                MatchDate = matchData.MatchDate,
                Round = matchData.Round,
                AvailableCredits = matchData.Credits,
                IsKnockout = matchData.IsKnockout,
                HeadToHead = h2h
            };
        }

        public async Task<FixturesViewModel> CreateFixturesViewModel(int leagueId, int? season, CancellationToken cancellationToken)
        {
            await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var league = await db.Leagues
                .Where(l => l.ApiLeagueId == leagueId)
                .Select(l => new { l.Id, l.Name, l.Logo })
                .FirstOrDefaultAsync(cancellationToken);

            if (league == null)
                throw new Exception($"League with id {leagueId} not found");

            var seasons = await db.Fixtures
                .Where(f => f.LeagueId == league.Id)
                .Select(f => f.Season)
                .Distinct()
                .OrderByDescending(s => s)
                .ToListAsync(cancellationToken);

            var selectedSeason = season ?? seasons.FirstOrDefault();
            var now = DateTimeOffset.UtcNow;

            var fixtures = await
                (
                    from fixture in db.Fixtures
                    join homeTeam in db.Teams
                        on fixture.HomeTeamId equals homeTeam.Id
                    join awayTeam in db.Teams
                        on fixture.AwayTeamId equals awayTeam.Id
                    where fixture.LeagueId == league.Id
                          && fixture.Season == selectedSeason
                    orderby fixture.Date

                    select new FixtureViewModel
                    {
                        Id = fixture.Id,
                        Date = fixture.Date,
                        Round = fixture.Round ?? string.Empty,
                        Status = fixture.Status ?? string.Empty,
                        HomeTeam = homeTeam.Name,
                        AwayTeam = awayTeam.Name,
                        HomeLogo = homeTeam.Logo,
                        AwayLogo = awayTeam.Logo,
                        HomeGoals = fixture.HomeGoals,
                        AwayGoals = fixture.AwayGoals,
                    }
                )
                .ToListAsync(cancellationToken);

            return new FixturesViewModel
            {
                LeagueId = leagueId,
                LeagueName = league.Name ?? "Unknown",
                LeagueLogo = league.Logo,
                Seasons = seasons,
                SelectedSeason = selectedSeason,
                UpcomingFixtures = fixtures.Where(f => f.Date > now).ToList(),
                PastFixtures = fixtures.Where(f => f.Date <= now).OrderByDescending(f => f.Date).ToList()
            };
        }

        public async Task<UserBetsViewModel> CreateUserBetsViewModel(Guid userId, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetUserBetsQuery { UserId = userId }, cancellationToken);

            var items = result.Items.Select(r => new UserBetItemViewModel
            {
                BetId = r.BetId,
                HomeTeam = r.HomeTeam,
                AwayTeam = r.AwayTeam,
                HomeLogo = r.HomeLogo,
                AwayLogo = r.AwayLogo,
                MatchDate = r.MatchDate,
                Prediction = r.Prediction,
                Credits = r.Credits,
                PlacedAt = r.PlacedAt,
                IsSettled = r.IsSettled,
                IsWon = r.IsWon,
                HomeGoals = r.HomeGoals,
                AwayGoals = r.AwayGoals,
                Payout = r.Payout,
                Reward = r.Reward
            }).ToList();

            var settledBets = items.Where(i => i.IsSettled).ToList();

            return new UserBetsViewModel
            {
                OpenBets = items.Where(i => !i.IsSettled).ToList(),
                SettledBets = settledBets,
                TotalBets = settledBets.Count,
                WonBets = settledBets.Count(b => b.IsWon == true),
                LostBets = settledBets.Count(b => b.IsWon == false),
                TotalWinnings = settledBets.Where(b => b.IsWon == true).Sum(b => b.Payout ?? 0),
                TotalStaked = settledBets.Sum(b => b.Credits)
            };
        }

        public async Task<TeamPlayersViewModel> CreateTeamPlayersViewModel(int apiTeamId, int season, CancellationToken cancellationToken)
        {
            var seasons = await mediator.Send(new GetTeamSeasonsQuery { ApiTeamId = apiTeamId }, cancellationToken);

            if (season == 0 && seasons.Any())
                season = seasons.Max();

            var players = await mediator.Send(new GetTeamPlayersQuery
            {
                ApiTeamId = apiTeamId,
                Season = season
            }, cancellationToken);

            return new TeamPlayersViewModel
            {
                ApiTeamId = apiTeamId,
                Season = season,
                AvailableSeasons = seasons.OrderByDescending(s => s),
                Players = players.Select(p => new PlayerViewModel
                {
                    Name = p.Name,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    BirthDate = p.BirthDate,
                    Photo = p.Photo,
                    Nationality = p.NationalityCountryId
                }).ToList()
            };
        }

        public async Task<ResultatIndexViewModel> CreateResultatIndexViewModel(CancellationToken cancellationToken)
        {
            var matches = await mediator.Send(new GetPlayedMatchesQuery(), cancellationToken);
            var teams = await nationalTeamRepository.GetAllAsync(cancellationToken);

            var model = new ResultatIndexViewModel
            {
                Matches = new List<ResultatMatchViewModel>()
            };

            foreach (var match in matches.OrderByDescending(m => m.Date))
            {
                var tips = await mediator.Send(new GetTipsByMatchQuery { MatchId = match.Id }, cancellationToken);
                var tipViewModels = new List<ResultatTipViewModel>();

                foreach (var tip in tips)
                {
                    var user = await userRepository.GetByIdAsync(tip.UserId, cancellationToken);
                    tipViewModels.Add(new ResultatTipViewModel
                    {
                        TipperName = user?.Name ?? "Unbekannt",
                        PredictedHome = tip.HomeGoals,
                        PredictedAway = tip.AwayGoals,
                        Points = tip.Points ?? 0
                    });
                }

                var home = teams.FirstOrDefault(t => t.Id == match.HomeNationalTeamId);
                var away = teams.FirstOrDefault(t => t.Id == match.AwayNationalTeamId);

                model.Matches.Add(new ResultatMatchViewModel
                {
                    HomeTeam = home?.Name ?? "Unknown",
                    AwayTeam = away?.Name ?? "Unknown",
                    HomeScore = match.HomeGoals!.Value,
                    AwayScore = match.AwayGoals!.Value,
                    KickoffUtc = match.Date,
                    WmGroup = match.WmGroup,
                    WmPhase = match.WmPhase!.Value,
                    Tips = tipViewModels.OrderByDescending(t => t.Points).ToList()
                });
            }

            return model;
        }

        public async Task<StandingsViewModel> CreateStandingsViewModel(int leagueId, int season, CancellationToken cancellationToken)
        {
            var standings = await leagueRepository.GetStandingsWithDetailsAsync(leagueId, season, cancellationToken);

            return new StandingsViewModel
            {
                LeagueId = leagueId,
                Season = season,
                Standings = standings.Select(s => new StandingViewModel
                {
                    Rank = s.Standing.Rank,
                    Points = s.Standing.Points,
                    GoalsDiff = s.Standing.GoalsDiff,
                    Team = new StandingTeamViewModel
                    {
                        Name = s.Team!.Name ?? "Unknown",
                        Logo = s.Team.Logo,
                        ApiTeamId = s.Team?.ApiTeamId
                    },
                    All = new StandingStatsViewModel
                    {
                        Played = s.Stats?.Played ?? 0,
                        Win = s.Stats?.Win ?? 0,
                        Draw = s.Stats?.Draw ?? 0,
                        Lose = s.Stats?.Lose ?? 0,
                        Goals = new StandingGoalsViewModel { For = 0, Against = 0 }
                    }
                })
                .OrderBy(s => s.Rank)
                .ToList()
            };
        }
    }
}