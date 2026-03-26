using FootballRadar.Business.Entities.LeagueEntities;

namespace FootballRadar.Abstractions
{
    public interface IMatchRepository
    {
        Task<Match?> GetByIdAsync(Guid id);
        Task AddAsync(Match match);
        Task<IReadOnlyCollection<Match>> GetUpcomingMatches();
        Task SaveChangesAsync();
        Task<IReadOnlyCollection<Match>> GetByLeagueAsync(int apiLeagueId, int season);
        Task<IReadOnlyCollection<int>> GetSeasonsByLeagueAsync(int apiLeagueId);
    }
}