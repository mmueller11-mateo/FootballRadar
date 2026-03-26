using FootballRadar.Business.Entities.Betting;

namespace FootballRadar.Abstractions
{
    public interface IBetRepository
    {
        Task<bool> HasUserBetOnMatchAsync(Guid userId, Guid matchId);
        Task<bool> HasUserBetOnMarketAsync(Guid userId, Guid marketId);
        Task AddBetAsync(Bet bet);
    }
}