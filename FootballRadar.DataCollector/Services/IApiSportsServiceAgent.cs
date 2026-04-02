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
        Task<IReadOnlyCollection<CountryResponse>> GetCountriesAsync();
        Task<IReadOnlyCollection<FixtureResponse>> GetFixturesAsync(int leagueId, int season);
        Task<IReadOnlyCollection<LeagueResponse>> GetLeaguesAsync();
        Task<IReadOnlyCollection<Standing>> GetStandingsAsync(int leagueId, int season);
        Task<IReadOnlyCollection<TeamResponse>> GetTeamsAsync(int leagueId, int season);
        Task<IReadOnlyCollection<TeamInfo>> GetTeamsByLeagueAsync(int leagueId, int season);
        Task<IReadOnlyCollection<PlayerResponse>> GetPlayersAsync(int teamId, int season);
    }
}