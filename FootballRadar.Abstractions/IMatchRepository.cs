using FootballRadar.Business.Entities.LeagueEntities;

namespace FootballRadar.Abstractions
{
    public interface IMatchRepository
    {
        Task<Match?> GetByIdAsync(Guid id);
        Task AddAsync(Match match);
        Task<IEnumerable<Match>> GetUpcomingMatches();
        Task<IEnumerable<Match>> GetByLeagueAsync(int apiLeagueId, int season);
        Task<IEnumerable<int>> GetSeasonsByLeagueAsync(int apiLeagueId);
        Task<IEnumerable<Match>> GetHeadToHeadAsync(Guid homeTeamId, Guid awayTeamId, int limit = 5);
    }
}