using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.TeamEntities;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Data.Repositories
{
    sealed class TeamRepository : ITeamRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> dbContextFactory;

        public TeamRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task AddAsync(Team team, CancellationToken cancellationToken = default)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            await dbContext.Teams.AddAsync(team, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Team>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await dbContext.Teams.ToListAsync(cancellationToken);
        }

        public async Task<Team?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await dbContext.Teams.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Team?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await dbContext.Teams.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower(), cancellationToken);
        }

        public void Update(Team team)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            dbContext.Teams.Update(team);
            dbContext.SaveChanges();
        }

        public void Delete(Team team)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            dbContext.Teams.Remove(team);
            dbContext.SaveChanges();
        }

        public async Task<IEnumerable<Team>> GetBySeasonAsync(int season, CancellationToken cancellationToken = default)
        {
            using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);

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