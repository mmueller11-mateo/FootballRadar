using FootballRadar.Business.Entities.LeagueEntities;
using System.Threading;

namespace FootballRadar.Abstractions
{
    public interface ILeagueRepository
    {
        Task<IEnumerable<PublicLeague>> GetLeaguesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Standing>> GetStandingsAsync(int apiLeagueId, int season, CancellationToken cancellationToken = default);
        Task<IEnumerable<StandingWithDetails>> GetStandingsWithDetailsAsync(int apiLeagueId, int season, CancellationToken cancellationToken = default);
        Task<IEnumerable<League>> GetTopLeaguesAsync(CancellationToken cancellationToken = default);
    }
}