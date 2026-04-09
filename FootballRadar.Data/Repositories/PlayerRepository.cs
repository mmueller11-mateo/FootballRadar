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

        public async Task<IReadOnlyCollection<Player>> GetByApiTeamIdAsync(int apiTeamId)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            return await dbContext.Players
                .Where(p => p.ApiTeamId == apiTeamId)
                .OrderBy(p => p.LastName)
                .ToListAsync();
        }
    }
}
