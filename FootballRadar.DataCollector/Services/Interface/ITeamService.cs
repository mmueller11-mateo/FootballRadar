using FootballRadar.DataCollector.FootballAPI.Models.Team;

namespace FootballRadar.DataCollector.Services.Interface
{
    public interface ITeamService
    {
        Task<IReadOnlyCollection<TeamResponse>> GetTeamsAsync(int leagueId, int season);
    }
}
