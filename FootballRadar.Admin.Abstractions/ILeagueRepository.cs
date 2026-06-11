using FootballRadar.Business.Entities.LeagueEntities;

namespace FootballRadar.Admin.Abstractions
{
    public interface ILeagueRepository
    {
        Task AddAsync(PublicLeague publicLeague, CancellationToken cancellationToken);
        Task<IEnumerable<PublicLeague>> GetAllAsync(CancellationToken cancellationToken);
        Task<PublicLeague?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<PublicLeague?> GetByNameAsync(string name, CancellationToken cancellationToken);
        void Update(PublicLeague publicLeague, CancellationToken cancellationToken);
        void Delete(PublicLeague publicLeague, CancellationToken cancellationToken);
    }
}
