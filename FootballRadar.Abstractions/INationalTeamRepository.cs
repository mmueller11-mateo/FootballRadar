using FootballRadar.Business.Entities.TeamEntities;

namespace FootballRadar.Abstractions
{
    public interface INationalTeamRepository
    {
        Task<IEnumerable<NationalTeam>> GetAllAsync(CancellationToken cancellationToken);
        Task<NationalTeam?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}