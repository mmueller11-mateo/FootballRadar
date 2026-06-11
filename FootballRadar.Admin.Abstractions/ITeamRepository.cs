using FootballRadar.Business.Entities.TeamEntities;

namespace FootballRadar.Admin.Abstractions
{
    public interface ITeamRepository
    {
        Task AddAsync(Team team, CancellationToken cancellationToken);
        Task<IEnumerable<Team>> GetAllAsync(CancellationToken cancellationToken);
        Task<Team?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Team?> GetByNameAsync(string name, CancellationToken cancellationToken);
        void Update(Team team, CancellationToken cancellationToken);
        void Delete(Team team, CancellationToken cancellationToken);
    }
}
