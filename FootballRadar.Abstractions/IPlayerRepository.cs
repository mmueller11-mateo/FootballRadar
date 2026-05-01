using FootballRadar.Business.Entities.PlayerIEntities;

namespace FootballRadar.Abstractions
{
    public interface IPlayerRepository
    {
        Task<IEnumerable<Player>> GetByApiTeamIdAsync(int apiTeamId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Player>> GetByTeamAndSeasonAsync(int apiTeamId, int season, CancellationToken cancellationToken = default);
        Task<IEnumerable<int>> GetSeasonsByTeamAsync(int apiTeamId, CancellationToken cancellationToken = default);
    }
}
