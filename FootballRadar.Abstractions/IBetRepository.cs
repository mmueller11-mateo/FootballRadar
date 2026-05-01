using FootballRadar.Business.Entities.Betting;
namespace FootballRadar.Abstractions
{
    public interface IBetRepository
    {
        Task<bool> HasUserBetOnMatchAsync(Guid userId, Guid matchId, CancellationToken cancellationToken = default);
        Task<bool> HasUserBetOnMarketAsync(Guid userId, Guid marketId, CancellationToken cancellationToken = default);
        Task AddBetAsync(Bet bet, CancellationToken cancellationToken = default);
        Task<PredictionMarket?> FindPredictionMarketForMatchAsync(Guid matchId, CancellationToken cancellationToken = default);
        Task<IEnumerable<MatchBet>> GetMatchBetsByMarketIdAsync(Guid marketId, CancellationToken cancellationToken = default);
        Task<IEnumerable<MatchBet>> GetMatchBetsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}