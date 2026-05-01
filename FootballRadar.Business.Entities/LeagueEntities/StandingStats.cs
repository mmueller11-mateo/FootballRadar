namespace FootballRadar.Business.Entities.LeagueEntities
{
    public class StandingStats
    {
        public Guid Id { get; set; }
        public int Played { get; set; }
        public int Win { get; set; }
        public int Draw { get; set; }
        public int Lose { get; set; }
    }
}
