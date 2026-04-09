using FootballRadar.DataCollector.ApiSports.FootballAPI;
using FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Country;
using FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Fixture;
using FootballRadar.DataCollector.ApiSports.FootballAPI.Models.League;
using FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Player;
using FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Standing;
using FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Team;

namespace FootballRadar.DataCollector.ApiSports.Services
{
    internal sealed class ApiSportsServiceAgent : IApiSportsServiceAgent
    {
        private readonly IApiSportsClient client;

        public ApiSportsServiceAgent(IApiSportsClient apiSportsClient)
        {
            client = apiSportsClient;
        }

        public async Task<IReadOnlyCollection<CountryResponse>> GetCountriesAsync()
        {
            var response = await client.GetCountriesAsync();
            return response.Response;
        }

        public async Task<IReadOnlyCollection<FixtureResponse>> GetFixturesAsync(int leagueId, int season)
        {
            var response = await client.GetFixturesAsync(leagueId, season);
            return response.Response;
        }

        public async Task<IReadOnlyCollection<LeagueResponse>> GetLeaguesAsync()
        {
            var response = await client.GetLeaguesAsync();
            return response.Response;
        }

        public async Task<IReadOnlyCollection<TeamInfo>> GetTeamsByLeagueAsync(int leagueId, int season)
        {
            var response = await client.GetTeamsByLeagueAsync(leagueId, season);
            return response.Response.Select(r => r.Team).ToList();
        }

        public async Task<IReadOnlyCollection<Standing>> GetStandingsAsync(int leagueId, int season)
        {
            var response = await client.GetStandingsAsync(leagueId, season);
            return response.Response.FirstOrDefault()?.League.Standings ?? new List<Standing>();
        }

        public async Task<IReadOnlyCollection<TeamResponse>> GetTeamsAsync(int leagueId, int season)
        {
            var response = await client.GetTeamsByLeagueAsync(leagueId, season);
            return response.Response;
        }

        public async Task<IReadOnlyCollection<PlayerResponse>> GetPlayersAsync(int teamId, int season)
        {
            var allPlayers = new List<PlayerResponse>();
            int page = 1;

            while (true)
            {
                var response = await client.GetPlayersAsync(teamId, season, page);

                if (response.Response == null || response.Response.Count == 0)
                    break;

                allPlayers.AddRange(response.Response);

                if (page >= response.Paging.Total)
                    break;

                page++;
                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            return allPlayers;
        }
    }
}
