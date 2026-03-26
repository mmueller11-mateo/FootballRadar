using FootballRadar.DataCollector.FootballAPI.Models;
using FootballRadar.DataCollector.FootballAPI.Models.Country;
using FootballRadar.DataCollector.FootballAPI.Models.Fixture;
using FootballRadar.DataCollector.FootballAPI.Models.League;
using FootballRadar.DataCollector.FootballAPI.Models.Standing;
using FootballRadar.DataCollector.FootballAPI.Models.Team;
using Refit;

namespace FootballRadar.DataCollector.FootballAPI
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
    }
}
