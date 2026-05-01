using FootballRadar.Business.Entities.Betting;

namespace FootballRadar.Abstractions
{
    public interface IPredictionMarketRepository
    {
        Task<PredictionMarket?> FindForMatchAsync(Guid matchId, CancellationToken cancellationToken = default);
        Task AddAsync(PredictionMarket predictionMarket, CancellationToken cancellationToken = default);
        Task<PredictionMarket?> FindForTransferRumorAsync(Guid transferRumorId, CancellationToken cancellationToken = default);
        Task UpdateAsync(PredictionMarket predictionMarket, CancellationToken cancellationToken = default);
        Task<PredictionMarket?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
