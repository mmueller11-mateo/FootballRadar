using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Data.ORM;
using FootballRadar.Business.Entities.LeagueEntities;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Admin.Data.Repositories
{
    sealed class LeagueRepository : ILeagueRepository
    {
        private readonly IDbContextFactory<AdminDbContext> _dbContextFactory;

        public LeagueRepository(IDbContextFactory<AdminDbContext> dbContextFactory)
        {
            this._dbContextFactory = dbContextFactory;
        }

        public async Task AddAsync(PublicLeague country)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            await dbContext.Leagues.AddAsync(country);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<PublicLeague>> GetAllAsync()
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            return await dbContext.Leagues.ToListAsync();

        }

        public async Task<PublicLeague?> GetByIdAsync(Guid id)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            return await dbContext.Leagues.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PublicLeague?> GetByNameAsync(string name)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            return await dbContext.Leagues.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public void Update(PublicLeague country)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            dbContext.Leagues.Update(country);
            dbContext.SaveChanges();
        }

        public void Delete(PublicLeague country)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            dbContext.Leagues.Remove(country);
            dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            await dbContext.SaveChangesAsync();
        }
    }
}
