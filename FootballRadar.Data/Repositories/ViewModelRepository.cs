using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting.Enums;
using FootballRadar.Business.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Data.Repositories
{
    internal sealed class ViewModelRepository : IViewModelRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> dbContextFactory;

        public ViewModelRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<PlaceBetViewModel> CreatePlaceBetViewModel(Guid matchId, Guid userId, CancellationToken cancellationToken)
        {
            await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var matchData = await (
                from match in db.Fixtures.AsNoTracking()
                join homeTeam in db.Teams on match.HomeTeamId equals homeTeam.Id
                join awayTeam in db.Teams on match.AwayTeamId equals awayTeam.Id
                where match.Id == matchId
                select new
                {
                    MatchId = match.Id,
                    HomeTeamId = match.HomeTeamId,
                    AwayTeamId = match.AwayTeamId,
                    HomeTeamName = homeTeam.Name,
                    AwayTeamName = awayTeam.Name,
                    HomeLogo = homeTeam.Logo,
                    AwayLogo = awayTeam.Logo,
                    MatchDate = match.Date,
                    Round = match.Round,
                    Credits = db.Wallets
                        .Where(w => w.UserId == userId)
                        .Select(w => w.Credits)
                        .FirstOrDefault()
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
    }
}