using FootballRadar.Business.Entities.TeamEntities;

namespace FootballRadar.Abstractions
{
    public interface ITeamRepository
    {
        Task AddAsync(Team team, CancellationToken cancellationToken = default);
        Task<IEnumerable<Team>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Team?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Team?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

        Task<IEnumerable<Team>> GetBySeasonAsync(int season, CancellationToken cancellationToken = default);

        void Update(Team team);
        void Delete(Team team);
    }
}
