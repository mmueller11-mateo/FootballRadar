using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Data.ORM;
using FootballRadar.Business.Entities.TeamEntities;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Admin.Data.Repositories
{
    sealed class TeamRepository : ITeamRepository
    {
        private readonly IDbContextFactory<AdminDbContext> _dbContextFactory;

        public TeamRepository(IDbContextFactory<AdminDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task AddAsync(Team team)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            await dbContext.Teams.AddAsync(team);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<Team>> GetAllAsync()
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

        public async Task SaveChangesAsync()
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            await dbContext.SaveChangesAsync();
        }
    }
}