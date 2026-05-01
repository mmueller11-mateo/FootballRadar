using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.TeamEntities;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Data.Repositories
{
    sealed class TeamRepository : ITeamRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public TeamRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task AddAsync(Team team)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            await dbContext.Teams.AddAsync(team);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Team>> GetAllAsync(CancellationToken cancellationToken)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            return await dbContext.Teams.ToListAsync();
        }

        public async Task<Team?> GetByIdAsync(Guid id)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            return await dbContext.Teams.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Team?> GetByNameAsync(string name)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            return await dbContext.Teams.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public void Update(Team team)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            dbContext.Teams.Update(team);
            dbContext.SaveChanges();
        }

        public void Delete(Team team)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            dbContext.Teams.Remove(team);
            dbContext.SaveChanges();
        }
        public async Task<IEnumerable<Team>> GetBySeasonAsync(int season, CancellationToken cancellationToken)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

            return await db.TeamSeasonPlayers
                .Where(x => x.Season == season)
                .Select(x => x.ApiTeamId)
                .Distinct()
                .Join(db.Teams,
                    apiId => apiId,
                    t => t.ApiTeamId,
                    (apiId, team) => team)
                .ToListAsync(cancellationToken);
        }
    }
}