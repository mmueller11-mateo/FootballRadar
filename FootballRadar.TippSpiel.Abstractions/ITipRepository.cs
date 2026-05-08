using FootballRadar.Business.Entities.TippSpiel;

namespace FootballRadar.TippSpiel.Abstractions
{
    public interface ITipRepository
    {
        Task<IEnumerable<Tip>> GetByTipperIdAsync(Guid tipperId);
        Task<IEnumerable<Tip>> GetByMatchIdAsync(Guid matchId);
        Task<Tip?> GetByTipperAndMatchAsync(Guid tipperId, Guid matchId);
        Task AddAsync(Tip tip);
        Task UpdateAsync(Tip tip);
    }
}
