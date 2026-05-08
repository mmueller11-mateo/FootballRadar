using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Data.Repositories
{
    sealed class PredictionMarketRepository : IPredictionMarketRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> dbContextFactory;

        public PredictionMarketRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<PredictionMarket?> FindForMatchAsync(Guid matchId, CancellationToken cancellationToken = default)
        {
            using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await db.MatchPredictionMarkets.SingleOrDefaultAsync(p => p.MatchId == matchId, cancellationToken);
        }

        public async Task AddAsync(PredictionMarket predictionMarket, CancellationToken cancellationToken = default)
        {
            using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            await db.PredictionMarkets.AddAsync(predictionMarket, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task<PredictionMarket?> FindForTransferRumorAsync(Guid transferRumorId, CancellationToken cancellationToken = default)
        {
            using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await db.PredictionMarkets
                .OfType<TransferPredictionMarket>()
                .FirstOrDefaultAsync(p => p.TransferRumorId == transferRumorId, cancellationToken);
        }

        public async Task UpdateAsync(PredictionMarket predictionMarket, CancellationToken cancellationToken = default)
        {
            using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            db.PredictionMarkets.Update(predictionMarket);
            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task<PredictionMarket?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await db.PredictionMarkets.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }
    }
}