using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.TippSpiel.Abstractions;
using FootballRadar.TippSpiel.Data;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Tippspiel.Data.Repositories
{
    internal class TipperRepository : ITipperRepository
    {
        private readonly IDbContextFactory<TippSpielDbContext> dbContextFactory;

        public TipperRepository(IDbContextFactory<TippSpielDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<TippUser?> GetByNameAsync(string name)
        {
            using var context = await dbContextFactory.CreateDbContextAsync();
            return await context.TippUsers.FirstOrDefaultAsync(t => t.Name == name);
        }

        public async Task<TippUser?> GetByIdAsync(Guid id)
        {
            using var context = await dbContextFactory.CreateDbContextAsync();
            return await context.TippUsers.FindAsync(id);
        }

        public async Task AddAsync(TippUser tipper)
        {
            using var context = await dbContextFactory.CreateDbContextAsync();
            context.TippUsers.Add(tipper);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TippUser>> GetAllAsync()
        {
            using var context = await dbContextFactory.CreateDbContextAsync();
            return await context.TippUsers.ToListAsync();
        }
    }
}
