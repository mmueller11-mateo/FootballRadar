using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Data.Repositories
{
    sealed class PredictionMarketRepository : IPredictionMarketRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public PredictionMarketRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<PredictionMarket?> FindForMatchAsync(Guid matchId)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            return await db.MatchPredictionMarkets.SingleOrDefaultAsync(p => p.MatchId == matchId);
        }



        public async Task AddAsync(PredictionMarket predictionMarket)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            await db.PredictionMarkets.AddAsync(predictionMarket);
            await db.SaveChangesAsync();
        }

        public async Task<PredictionMarket?> FindForTransferRumorAsync(Guid transferRumorId)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            return await db.PredictionMarkets
                .OfType<TransferPredictionMarket>()
                .FirstOrDefaultAsync(p => p.TransferRumorId == transferRumorId);
        }
    }
}