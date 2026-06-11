using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Data.ORM;
using FootballRadar.Business.Entities.TeamEntities;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Admin.Data.Repositories
{
    sealed class TeamRepository : ITeamRepository
    {
        private readonly IDbContextFactory<AdminDbContext> dbContextFactory;

        public TeamRepository(IDbContextFactory<AdminDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task AddAsync(Team team, CancellationToken cancellationToken)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            await dbContext.Teams.AddAsync(team);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Team>> GetAllAsync(CancellationToken cancellationToken)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.Teams.ToListAsync();
        }

        public async Task<Team?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.Teams.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Team?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.Teams.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public void Update(Team team, CancellationToken cancellationToken)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            dbContext.Teams.Update(team);
            dbContext.SaveChanges();
        }

        public void Delete(Team team, CancellationToken cancellationToken)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            dbContext.Teams.Remove(team);
            dbContext.SaveChanges();
        }

    }
}