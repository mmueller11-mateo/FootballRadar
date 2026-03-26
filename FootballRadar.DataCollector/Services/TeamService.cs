using FootballRadar.DataCollector.FootballAPI;
using FootballRadar.DataCollector.FootballAPI.Models.Team;
using FootballRadar.DataCollector.Services.Interface;

namespace FootballRadar.DataCollector.Services
{
    internal class TeamService : ITeamService
    {
        private readonly IApiSportsClient client;

        public TeamService(IApiSportsClient apiSportsClient)
        {
            client = apiSportsClient;
        }

        public async Task<IReadOnlyCollection<TeamResponse>> GetTeamsAsync(int leagueId, int season)
        {
            var response = await client.GetTeamsByLeagueAsync(leagueId, season);
            return response.Response;
        }
    }
}
