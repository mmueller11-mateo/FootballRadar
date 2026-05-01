namespace FootballRadar.Business.Entities.PlayerIEntities
{
    public class TeamSeasonPlayer
    {
        public Guid Id { get; set; }

        public int Season { get; set; }

        public int ApiTeamId { get; set; }
        public int ApiPlayerId { get; set; }

        public Guid PlayerId { get; set; }
        public Player Player { get; set; } = null!;

    }
}
