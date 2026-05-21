using global::FootballRadar.Abstractions;
using global::FootballRadar.Business.Entities.LeagueEntities;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Data.Repositories
{
    sealed class MatchRepository : IMatchRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> dbContextFactory;

        public MatchRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<IEnumerable<Match>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await db.Fixtures.ToArrayAsync(cancellationToken);
        }

        public async Task<Match?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await db.Fixtures.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public async Task AddAsync(Match match, CancellationToken cancellationToken = default)
        {
            using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            await db.Fixtures.AddAsync(match, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Match>> GetUpcomingMatches(CancellationToken cancellationToken = default)
        {
            using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await db.Fixtures.ToArrayAsync(cancellationToken);
        }

        public async Task<IEnumerable<Match>> GetByLeagueAsync(int apiLeagueId, int season, CancellationToken cancellationToken = default)
        {
            using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var league = await db.Leagues.FirstOrDefaultAsync(l => l.ApiLeagueId == apiLeagueId, cancellationToken);
            if (league == null)
                return Array.Empty<Match>();

            return await db.Fixtures
                .Where(f => f.LeagueId == league.Id && f.Season == season)
                .OrderBy(f => f.Date)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<int>> GetSeasonsByLeagueAsync(int apiLeagueId, CancellationToken cancellationToken = default)
        {
            using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var league = await db.Leagues.FirstOrDefaultAsync(l => l.ApiLeagueId == apiLeagueId, cancellationToken);
            if (league == null)
                return Array.Empty<int>();
            return await db.Fixtures
                .Where(f => f.LeagueId == league.Id)
                .Select(f => f.Season)
                .Distinct()
                .OrderByDescending(s => s)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Match>> GetHeadToHeadAsync(Guid homeTeamId, Guid awayTeamId, int limit = 5, CancellationToken cancellationToken = default)
        {
            using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await db.Fixtures
                .Where(f =>
                    (f.HomeTeamId == homeTeamId && f.AwayTeamId == awayTeamId) ||
                    (f.HomeTeamId == awayTeamId && f.AwayTeamId == homeTeamId))
                .Where(f => f.HomeGoals.HasValue && f.AwayGoals.HasValue)
                .OrderByDescending(f => f.Date)
                .Take(limit)
                .ToListAsync(cancellationToken);
        }
    }
}
