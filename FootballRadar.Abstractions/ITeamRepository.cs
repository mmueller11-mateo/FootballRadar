using FootballRadar.Business.Entities.TeamEntities;

namespace FootballRadar.Abstractions
{
    public interface ITeamRepository
    {
        Task AddAsync(Team team);
        Task<IEnumerable<Team>> GetAllAsync(CancellationToken cancellationToken);
        Task<Team?> GetByIdAsync(Guid id);
        Task<Team?> GetByNameAsync(string name);

        Task<IEnumerable<Team>> GetBySeasonAsync(int season, CancellationToken cancellationToken);

        void Update(Team team);
        void Delete(Team team);
    }
}
