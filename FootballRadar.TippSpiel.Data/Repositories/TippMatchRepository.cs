using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.TippSpiel.Abstractions;
using FootballRadar.TippSpiel.Data;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Tippspiel.Data.Repositories
{
    public class TippMatchRepository : ITippMatchRepository
    {
        private readonly IDbContextFactory<TippspielDbContext> dbContextFactory;

        public TippMatchRepository(IDbContextFactory<TippspielDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<IEnumerable<TippMatch>> GetAllAsync()
        {
            using var context = await dbContextFactory.CreateDbContextAsync();
            return await context.TippMatches.ToListAsync();
        }

        public async Task<TippMatch?> GetByIdAsync(Guid id)
        {
            using var context = await dbContextFactory.CreateDbContextAsync();
            return await context.TippMatches.FindAsync(id);
        }

        public async Task AddAsync(TippMatch match)
        {
            using var context = await dbContextFactory.CreateDbContextAsync();
            context.TippMatches.Add(match);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TippMatch match)
        {
            using var context = await dbContextFactory.CreateDbContextAsync();
            context.TippMatches.Update(match);
            await context.SaveChangesAsync();
        }
    }
}
