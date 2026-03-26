using FootballRadar.DataCollector.FootballAPI.Models.League;
using FootballRadar.DataCollector.FootballAPI.Models.Standing;
using FootballRadar.DataCollector.FootballAPI.Models.Team;

namespace FootballRadar.DataCollector.Services.Interface
{
    public interface ILeagueService
    {
        Task<IReadOnlyCollection<LeagueResponse>> GetLeaguesAsync();
        Task<IReadOnlyCollection<TeamInfo>> GetTeamsByLeagueAsync(int leagueId, int season);
        Task<IReadOnlyCollection<Standing>> GetStandingsAsync(int leagueId, int season);
    }
}
