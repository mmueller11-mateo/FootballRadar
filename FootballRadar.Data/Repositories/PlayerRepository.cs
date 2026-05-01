using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.PlayerIEntities;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Data.Repositories
{
    sealed class PlayerRepository : IPlayerRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public PlayerRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<IEnumerable<Player>> GetByApiTeamIdAsync(int apiTeamId, CancellationToken cancellationToken = default)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

            return await dbContext.TeamSeasonPlayers
                .Where(x => x.ApiTeamId == apiTeamId)
                .Select(x => x.Player)
                .OrderBy(p => p.LastName)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Player>> GetByTeamAndSeasonAsync(int apiTeamId, int season, CancellationToken cancellationToken = default)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

            return await dbContext.TeamSeasonPlayers
                .Where(x => x.ApiTeamId == apiTeamId && x.Season == season)
                .Select(x => x.Player)
                .OrderBy(p => p.LastName)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<int>> GetSeasonsByTeamAsync(int apiTeamId, CancellationToken cancellationToken = default)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await dbContext.TeamSeasonPlayers
                .Where(x => x.ApiTeamId == apiTeamId)
                .Select(x => x.Season)
                .Distinct()
                .OrderByDescending(s => s)
                .ToListAsync(cancellationToken);
        }
    }
}