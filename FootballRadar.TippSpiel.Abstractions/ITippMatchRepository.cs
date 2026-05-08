using FootballRadar.Business.Entities.TippSpiel;

namespace FootballRadar.TippSpiel.Abstractions
{
    public interface ITippMatchRepository
    {
        Task<IEnumerable<TippMatch>> GetAllAsync();
        Task<TippMatch?> GetByIdAsync(Guid id);
        Task AddAsync(TippMatch match);
        Task UpdateAsync(TippMatch match);
    }
}
