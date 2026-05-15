using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.TippSpiel.Abstractions;
using FootballRadar.TippSpiel.Data;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Tippspiel.Data.Repositories
{
    public class TipRepository : ITipRepository
    {
        private readonly IDbContextFactory<TippSpielDbContext> dbContextFactory;

        public TipRepository(IDbContextFactory<TippSpielDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<IEnumerable<Tip>> GetByTipperIdAsync(Guid tipperId)
        {
            using var context = await dbContextFactory.CreateDbContextAsync();
            return await context.Tips.Where(t => t.TipperId == tipperId).ToListAsync();
        }

        public async Task<IEnumerable<Tip>> GetByMatchIdAsync(Guid matchId)
        {
            using var context = await dbContextFactory.CreateDbContextAsync();
            return await context.Tips.Where(t => t.MatchId == matchId).ToListAsync();
        }

        public async Task<Tip?> GetByTipperAndMatchAsync(Guid tipperId, Guid matchId)
        {
            using var context = await dbContextFactory.CreateDbContextAsync();
            return await context.Tips.FirstOrDefaultAsync(t => t.TipperId == tipperId && t.MatchId == matchId);
        }

        public async Task AddAsync(Tip tip)
        {
            using var context = await dbContextFactory.CreateDbContextAsync();
            context.Tips.Add(tip);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tip tip)
        {
            using var context = await dbContextFactory.CreateDbContextAsync();
            context.Tips.Update(tip);
            await context.SaveChangesAsync();
        }
    }
}
