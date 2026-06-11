using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Data.ORM;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Admin.Data.Repositories
{
    internal class WmTipRepository : IWmTipRepository
    {
        private readonly IDbContextFactory<AdminDbContext> context;

        public WmTipRepository(IDbContextFactory<AdminDbContext> context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<WmTip>> GetByUserIdAsync(
            Guid userId, CancellationToken cancellationToken)
        {
            using var context = await this.context.CreateDbContextAsync();
            return await context.WmTips
                .Where(t => t.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<WmTip>> GetByMatchIdAsync(
            Guid matchId, CancellationToken cancellationToken)
        {
            using var context = await this.context.CreateDbContextAsync();
            return await context.WmTips
                .Where(t => t.WmMatchId == matchId)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(WmTip tip, CancellationToken cancellationToken)
        {
            using var context = await this.context.CreateDbContextAsync();
            context.WmTips.Update(tip);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task SaveTipAsync(WmTip tip, CancellationToken cancellationToken)
        {
            using var context = await this.context.CreateDbContextAsync();
            var existing = await context.WmTips
                .FirstOrDefaultAsync(
                    t => t.UserId == tip.UserId && t.WmMatchId == tip.WmMatchId,
                    cancellationToken);

            if (existing == null)
                await context.WmTips.AddAsync(tip, cancellationToken);
            else
            {
                existing.HomeGoals = tip.HomeGoals;
                existing.AwayGoals = tip.AwayGoals;
                existing.SubmittedAt = tip.SubmittedAt;
                context.WmTips.Update(existing);
            }

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
