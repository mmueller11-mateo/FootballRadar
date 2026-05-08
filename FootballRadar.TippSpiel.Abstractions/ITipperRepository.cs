using FootballRadar.Business.Entities.TippSpiel;

namespace FootballRadar.TippSpiel.Abstractions
{
    public interface ITipperRepository
    {
        Task<Tipper?> GetByNameAsync(string name);
        Task<Tipper?> GetByIdAsync(Guid id);
        Task AddAsync(Tipper tipper);
        Task<IEnumerable<Tipper>> GetAllAsync();
    }
}
