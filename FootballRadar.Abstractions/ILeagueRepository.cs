using FootballRadar.Business.Entities.LeagueEntities;

namespace FootballRadar.Abstractions
{
    public interface ILeagueRepository
    {
        Task<IReadOnlyCollection<PublicLeague>> GetLeaguesAsync();
        Task<IReadOnlyCollection<Standing>> GetStandingsAsync(int apiLeagueId, int season);
        Task<IReadOnlyCollection<StandingWithDetails>> GetStandingsWithDetailsAsync(int apiLeagueId, int season);

        Task<IReadOnlyCollection<League>> GetTopLeaguesAsync();

    }
}