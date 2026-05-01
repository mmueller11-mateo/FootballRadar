using FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Country;
using FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Fixture;
using FootballRadar.DataCollector.ApiSports.FootballAPI.Models.League;
using FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Player;
using FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Standing;
using FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Team;

namespace FootballRadar.DataCollector.ApiSports.Services
{
    internal interface IApiSportsServiceAgent
    {
        Task<IEnumerable<CountryResponse>> GetCountriesAsync();
        Task<IEnumerable<FixtureResponse>> GetFixturesAsync(int leagueId, int season);
        Task<IEnumerable<LeagueResponse>> GetLeaguesAsync();
        Task<IEnumerable<Standing>> GetStandingsAsync(int leagueId, int season);
        Task<IEnumerable<TeamResponse>> GetTeamsAsync(int leagueId, int season);
        Task<IEnumerable<TeamInfo>> GetTeamsByLeagueAsync(int leagueId, int season);
        Task<IEnumerable<PlayerResponse>> GetPlayersAsync(int teamId, int season);
    }
}