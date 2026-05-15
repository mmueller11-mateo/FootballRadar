using FootballRadar.Business.Entities.TippSpiel;

namespace FootballRadar.TippSpiel.Abstractions
{
    public interface ITipperRepository
    {
        Task<TippUser?> GetByNameAsync(string name);
        Task<TippUser?> GetByIdAsync(Guid id);
        Task AddAsync(TippUser tipper);
        Task<IEnumerable<TippUser>> GetAllAsync();
    }
}
