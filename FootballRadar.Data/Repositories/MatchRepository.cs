using global::FootballRadar.Abstractions;
using global::FootballRadar.Business.Entities.LeagueEntities;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Data.Repositories
{
    sealed class MatchRepository : IMatchRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public MatchRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<Match?> GetByIdAsync(Guid id)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            return await db.Fixtures.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAsync(Match match)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            await db.Fixtures.AddAsync(match);
            await db.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            await db.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<Match>> GetUpcomingMatches()
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            return await db.Fixtures.ToArrayAsync();
        }

        public async Task<IReadOnlyCollection<Match>> GetByLeagueAsync(int apiLeagueId, int season)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();

            var league = await db.Leagues.FirstOrDefaultAsync(l => l.ApiLeagueId == apiLeagueId);
            if (league == null)
                return Array.Empty<Match>();

            return await db.Fixtures
                .Where(f => f.LeagueId == league.Id && f.Season == season)
                .OrderBy(f => f.Date)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<int>> GetSeasonsByLeagueAsync(int apiLeagueId)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            var league = await db.Leagues.FirstOrDefaultAsync(l => l.ApiLeagueId == apiLeagueId);
            if (league == null)
                return Array.Empty<int>();
            return await db.Fixtures
                .Where(f => f.LeagueId == league.Id)
                .Select(f => f.Season)
                .Distinct()
                .OrderByDescending(s => s)
                .ToListAsync();
        }
    }
}
