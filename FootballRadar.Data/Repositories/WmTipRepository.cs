using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.TippSpiel;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Data.Repositories
{
    internal class WmTipRepository : IWmTipRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> dbFactory;

        public WmTipRepository(IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public async Task<IEnumerable<WmTip>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            await using var context = await dbFactory.CreateDbContextAsync(cancellationToken);
            return await context.WmTips
                .Where(t => t.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task UpsertAsync(WmTip tip, CancellationToken cancellationToken)
        {
            await using var context = await dbFactory.CreateDbContextAsync(cancellationToken);

            var existing = await context.WmTips
                .FirstOrDefaultAsync(t => t.UserId == tip.UserId && t.WmMatchId == tip.WmMatchId, cancellationToken);

            if (existing is null)
            {
                tip.Id = Guid.NewGuid();
                tip.SubmittedAt = DateTimeOffset.UtcNow;
                await context.WmTips.AddAsync(tip, cancellationToken);
            }
            else
            {
                existing.HomeGoals = tip.HomeGoals;
                existing.AwayGoals = tip.AwayGoals;
                existing.SubmittedAt = DateTimeOffset.UtcNow;
                context.WmTips.Update(existing);
            }

            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<WmTip>> GetByMatchIdAsync(Guid wmMatchId, CancellationToken cancellationToken)
        {
            await using var context = await dbFactory.CreateDbContextAsync(cancellationToken);
            return await context.WmTips
                .Where(t => t.WmMatchId == wmMatchId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<WmTip>> GetAllAsync(CancellationToken cancellationToken)
        {
            await using var context = await dbFactory.CreateDbContextAsync(cancellationToken);
            return await context.WmTips.ToListAsync(cancellationToken);
        }
    }
}