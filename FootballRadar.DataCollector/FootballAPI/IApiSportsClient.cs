using FootballRadar.DataCollector.ApiSports.FootballAPI.Models;
using FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Country;
using FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Fixture;
using FootballRadar.DataCollector.ApiSports.FootballAPI.Models.League;
using FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Player;
using FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Standing;
using FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Team;
using Refit;

namespace FootballRadar.DataCollector.ApiSports.FootballAPI
{
    public interface IApiSportsClient
    {
        [Get("/leagues")]
        Task<ApiSportsResponse<LeagueResponse>> GetLeaguesAsync();

        [Get("/teams")]
        Task<ApiSportsResponse<TeamResponse>> GetTeamsByLeagueAsync([Query] int league, [Query] int season);

        [Get("/standings")]
        Task<ApiSportsResponse<StandingsResponse>> GetStandingsAsync([Query] int league, [Query] int season);

        [Get("/leagues")]
        Task<ApiSportsResponse<LeagueResponse>> GetLeaguesAsync([Query] string? name = null, [Query] string? country = null);

        [Get("/countries")]
        Task<ApiSportsResponse<CountryResponse>> GetCountriesAsync();


        [Get("/teams")]
        Task<string> GetTeamsRawAsync([Query] int league, [Query] int season);

        [Get("/fixtures")]
        Task<ApiSportsResponse<FixtureResponse>> GetFixturesAsync([Query] int league, [Query] int season);

        [Get("/players")]
        Task<ApiSportsResponse<PlayerResponse>> GetPlayersAsync([Query] int team, [Query] int season, [Query] int page = 1);
    }
}
