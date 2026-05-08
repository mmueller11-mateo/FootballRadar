using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.TippSpiel.Abstractions;
using FootballRadar.TippSpiel.Data;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Tippspiel.Data.Repositories
{
    internal class TipperRepository : ITipperRepository
    {
        private readonly IDbContextFactory<TippspielDbContext> dbContextFactory;

        public TipperRepository(IDbContextFactory<TippspielDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<Tipper?> GetByNameAsync(string name)
        {
            using var context = await dbContextFactory.CreateDbContextAsync();
            return await context.Tippers.FirstOrDefaultAsync(t => t.Name == name);
        }

        public async Task<Tipper?> GetByIdAsync(Guid id)
        {
            using var context = await dbContextFactory.CreateDbContextAsync();
            return await context.Tippers.FindAsync(id);
        }

        public async Task AddAsync(Tipper tipper)
        {
            using var context = await dbContextFactory.CreateDbContextAsync();
            context.Tippers.Add(tipper);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tipper>> GetAllAsync()
        {
            using var context = await dbContextFactory.CreateDbContextAsync();
            return await context.Tippers.ToListAsync();
        }
    }
}
