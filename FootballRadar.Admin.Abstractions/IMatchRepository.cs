using FootballRadar.Business.Entities.LeagueEntities;

namespace FootballRadar.Admin.Abstractions
{
    public interface IMatchRepository
    {
        Task<IEnumerable<Match>> GetWmMatchesAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Match>> GetByStatusAsync(string status, CancellationToken cancellationToken);
        Task<Match?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateAsync(Match match, CancellationToken cancellationToken);
        Task AddAsync(Match match, CancellationToken cancellationToken);
    }
}
