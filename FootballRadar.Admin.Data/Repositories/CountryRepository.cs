using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Data.ORM;
using FootballRadar.Business.Entities;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Admin.Data.Repositories
{
    sealed class CountryRepository : ICountryRepository
    {
        private readonly IDbContextFactory<AdminDbContext> _dbContextFactory;

        public CountryRepository(IDbContextFactory<AdminDbContext> dbContextFactory)
        {
            this._dbContextFactory = dbContextFactory;
        }

        public async Task AddAsync(Country country)
        {
            using var dbContext = await this._dbContextFactory.CreateDbContextAsync();
            await dbContext.Countries.AddAsync(country);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Country>> GetAllAsync()
        {
            using var dbContext = await this._dbContextFactory.CreateDbContextAsync();
            return await dbContext.Countries.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Country?> GetByIdAsync(Guid id)
        {
            using var dbContext = await this._dbContextFactory.CreateDbContextAsync();
            return await dbContext.Countries.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Country?> GetByNameAsync(string name)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            return await dbContext.Countries.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public void Update(Country country)
        {
            using var dbContext = this._dbContextFactory.CreateDbContext();
            dbContext.Countries.Update(country);
            dbContext.SaveChanges();
        }

        public void Delete(Country country)
        {
            using var dbContext = this._dbContextFactory.CreateDbContext();
            dbContext.Countries.Remove(country);
            dbContext.SaveChanges();
        }
    }
}
