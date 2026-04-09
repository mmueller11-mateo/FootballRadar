using FootballRadar.Business.Entities.PlayerIEntities;

namespace FootballRadar.Abstractions
{
    public interface IPlayerRepository
    {
        Task<IReadOnlyCollection<Player>> GetByApiTeamIdAsync(int apiTeamId);
    }
}
