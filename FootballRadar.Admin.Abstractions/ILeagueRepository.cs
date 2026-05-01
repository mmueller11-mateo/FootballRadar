using FootballRadar.Business.Entities.LeagueEntities;

namespace FootballRadar.Admin.Abstractions
{
    public interface ILeagueRepository
    {
        Task AddAsync(PublicLeague publicLeague);
        Task<IEnumerable<PublicLeague>> GetAllAsync();
        Task<PublicLeague?> GetByIdAsync(Guid id);
        Task<PublicLeague?> GetByNameAsync(string name);
        void Update(PublicLeague publicLeague);
        void Delete(PublicLeague publicLeague);
    }
}
