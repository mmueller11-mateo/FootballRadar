using FootballRadar.Business.Entities.Betting;

namespace FootballRadar.Abstractions
{
    public interface IPredictionMarketRepository
    {
        Task<PredictionMarket?> FindForMatchAsync(Guid matchId);
        Task<PredictionMarket?> FindForTransferRumorAsync(Guid transferRumorId);
        Task AddAsync(PredictionMarket predictionMarket);
    }
}
