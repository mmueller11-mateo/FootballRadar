using FootballRadar.DataCollector.FootballAPI;
using FootballRadar.DataCollector.FootballAPI.Models.League;
using FootballRadar.DataCollector.FootballAPI.Models.Standing;
using FootballRadar.DataCollector.FootballAPI.Models.Team;
using FootballRadar.DataCollector.Services.Interface;

namespace FootballRadar.DataCollector.Services
{
    internal class LeagueService : ILeagueService
    {
        private readonly IApiSportsClient client;

        public LeagueService(IApiSportsClient apiSportsClient)
        {
            client = apiSportsClient;
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
    }
}
