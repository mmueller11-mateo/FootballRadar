using FootballRadar.Business.Entities.Enums;

namespace FootballRadar.Business.Entities.PlayerIEntities
{
    public class PlayerPosition
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid TeamId { get; set; }
        public DateOnly FromDate { get; set; }
        public PlayerPositions Position { get; set; }
    }
}
