using FootballRadar.Business.Entities.LeagueEntities;

namespace FootballRadar.Abstractions
{
    public interface ILeagueRepository
    {
        Task<IEnumerable<PublicLeague>> GetLeaguesAsync();
        Task<IEnumerable<Standing>> GetStandingsAsync(int apiLeagueId, int season);
        Task<IEnumerable<StandingWithDetails>> GetStandingsWithDetailsAsync(int apiLeagueId, int season);
        Task<IEnumerable<League>> GetTopLeaguesAsync();
    }
}