using FootballRadar.Business.Entities.TeamEntities;

namespace FootballRadar.Admin.Abstractions
{
    public interface INationalTeamRepository
    {
        Task AddAsync(NationalTeam nationalTeam);
        Task<IEnumerable<NationalTeam>> GetAllAsync();
        Task<NationalTeam?> GetByIdAsync(Guid id);
        Task<NationalTeam?> GetByNameAsync(string name);
        void Update(NationalTeam nationalTeam);
        void Delete(NationalTeam nationalTeam);
    }
}