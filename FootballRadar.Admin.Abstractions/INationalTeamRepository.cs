using FootballRadar.Business.Entities.TeamEntities;

namespace FootballRadar.Admin.Abstractions
{
    public interface INationalTeamRepository
    {
        Task AddAsync(NationalTeam nationalTeam, CancellationToken cancellationToken);
        Task<IEnumerable<NationalTeam>> GetAllAsync(CancellationToken cancellationToken);
        Task<NationalTeam?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<NationalTeam?> GetByNameAsync(string name, CancellationToken cancellationToken);
        void Update(NationalTeam nationalTeam, CancellationToken cancellationToken);
        void Delete(NationalTeam nationalTeam, CancellationToken cancellationToken);
    }
}