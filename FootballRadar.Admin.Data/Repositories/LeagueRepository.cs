using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Data.ORM;
using FootballRadar.Business.Entities.LeagueEntities;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Admin.Data.Repositories
{
    sealed class LeagueRepository : ILeagueRepository
    {
        private readonly IDbContextFactory<AdminDbContext> dbContextFactory;

        public LeagueRepository(IDbContextFactory<AdminDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task AddAsync(PublicLeague country, CancellationToken cancellationToken)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            await dbContext.Leagues.AddAsync(country);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<PublicLeague>> GetAllAsync(CancellationToken cancellationToken)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.Leagues.ToListAsync();

        }

        public async Task<PublicLeague?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.Leagues.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PublicLeague?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.Leagues.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public void Update(PublicLeague country, CancellationToken cancellationToken)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            dbContext.Leagues.Update(country);
            dbContext.SaveChanges();
        }

        public void Delete(PublicLeague country, CancellationToken cancellationToken)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            dbContext.Leagues.Remove(country);
            dbContext.SaveChanges();
        }
    }
}
