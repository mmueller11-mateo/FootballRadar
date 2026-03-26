using FootballRadar.DataCollector.FootballAPI.Models.Fixture;

namespace FootballRadar.DataCollector.Services.Interface
{
    public interface IFixtureService
    {
        Task<IReadOnlyCollection<FixtureResponse>> GetFixturesAsync(int leagueId, int season);
    }
}
