using FootballRadar.Business.Entities.TippSpiel;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Data.Repositories
{
    internal sealed class BonusTipRepository : IBonusTipRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> dbContextFactory;

        public BonusTipRepository(IDbContextFactory<ApplicationDbContext> dbContext)
        {
            this.dbContextFactory = dbContext;
        }

        public async Task<IEnumerable<BonusTip>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await dbContext.BonusTips.AsNoTracking().Where(x => x.UserId == userId).ToListAsync(cancellationToken);
        }

        public async Task<BonusTip?> GetByUserAndQuestionAsync(Guid userId, Guid questionId, CancellationToken cancellationToken)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await dbContext.BonusTips.FirstOrDefaultAsync(x => x.UserId == userId && x.BonusQuestionId == questionId, cancellationToken);
        }

        public async Task<IEnumerable<BonusTip>> GetByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await dbContext.BonusTips.AsNoTracking().Where(x => x.BonusQuestionId == questionId).ToListAsync(cancellationToken);
        }

        public async Task UpsertAsync(BonusTip tip, CancellationToken cancellationToken)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var existing = await dbContext.BonusTips.FirstOrDefaultAsync(x => x.UserId == tip.UserId && x.BonusQuestionId == tip.BonusQuestionId, cancellationToken);

            if (existing is null)
            {
                await dbContext.BonusTips.AddAsync(tip, cancellationToken);
            }
            else
            {
                dbContext.Entry(existing).CurrentValues.SetValues(tip);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(BonusTip tip, CancellationToken cancellationToken)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            dbContext.BonusTips.Update(tip);

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<BonusTip>> GetAllAsync(CancellationToken cancellationToken)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await dbContext.BonusTips.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
