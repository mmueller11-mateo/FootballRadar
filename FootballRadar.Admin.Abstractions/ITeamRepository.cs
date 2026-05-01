using FootballRadar.Business.Entities.TeamEntities;

namespace FootballRadar.Admin.Abstractions
{
    public interface ITeamRepository
    {
        Task AddAsync(Team team);
        Task<IEnumerable<Team>> GetAllAsync();
        Task<Team?> GetByIdAsync(Guid id);
        Task<Team?> GetByNameAsync(string name);
        void Update(Team team);
        void Delete(Team team);
    }
}
