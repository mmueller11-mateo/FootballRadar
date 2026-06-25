using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.TippSpiel;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Data.Repositories
{
    internal sealed class BonusQuestionRepository : IBonusQuestionRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> dbContextFactory;

        public BonusQuestionRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<IEnumerable<BonusQuestion>> GetAllAsync(CancellationToken cancellationToken)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await dbContext.BonusQuestions
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<BonusQuestion?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await dbContext.BonusQuestions
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(BonusQuestion question, CancellationToken cancellationToken)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            dbContext.BonusQuestions.Update(question);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<BonusQuestion>> GetByIdsAsync(IReadOnlySet<Guid> ids, CancellationToken cancellationToken)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await dbContext.BonusQuestions
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(cancellationToken);
        }
    }
}
