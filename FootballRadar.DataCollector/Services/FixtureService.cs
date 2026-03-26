namespace FootballRadar.DataCollector.Services
{
    using global::FootballRadar.DataCollector.FootballAPI;
    using global::FootballRadar.DataCollector.FootballAPI.Models.Fixture;
    using global::FootballRadar.DataCollector.Services.Interface;

    namespace FootballRadar.DataCollector.Services
    {
        internal class FixtureService : IFixtureService
        {
            private readonly IApiSportsClient _client;

            public FixtureService(IApiSportsClient apiSportsClient)
            {
                _client = apiSportsClient;
            }

            public async Task<IReadOnlyCollection<FixtureResponse>> GetFixturesAsync(int leagueId, int season)
            {
                var response = await _client.GetFixturesAsync(leagueId, season);
                return response.Response;
            }
        }
    }
}
