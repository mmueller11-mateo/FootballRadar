using FootballRadar.Business.Entities.LeagueEntities;
using System.Threading;

namespace FootballRadar.Abstractions
{
    public interface IMatchRepository
    {
        Task<Match?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(Match match, CancellationToken cancellationToken = default);
        Task<IEnumerable<Match>> GetUpcomingMatches(CancellationToken cancellationToken = default);
        Task<IEnumerable<Match>> GetByLeagueAsync(int apiLeagueId, int season, CancellationToken cancellationToken = default);
        Task<IEnumerable<int>> GetSeasonsByLeagueAsync(int apiLeagueId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Match>> GetHeadToHeadAsync(Guid homeTeamId, Guid awayTeamId, int limit = 5, CancellationToken cancellationToken = default);
    }
}