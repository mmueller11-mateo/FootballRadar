using FootballRadar.Business.Entities.Betting;
namespace FootballRadar.Abstractions
{
    public interface IBetRepository
    {
        Task<bool> HasUserBetOnMatchAsync(Guid userId, Guid matchId, CancellationToken cancellationToken = default);
        Task<bool> HasUserBetOnMarketAsync(Guid userId, Guid marketId, CancellationToken cancellationToken = default);
        Task AddBetAsync(Bet bet, CancellationToken cancellationToken = default);
        Task<PredictionMarket?> FindPredictionMarketForMatchAsync(Guid matchId, CancellationToken cancellationToken = default);
        Task<IEnumerable<WinnerBet>> GetMatchBetsByMarketIdAsync(Guid marketId, CancellationToken cancellationToken = default);
        Task<IEnumerable<WinnerBet>> GetMatchBetsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Bet>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}