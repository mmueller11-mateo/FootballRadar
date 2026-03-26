namespace FootballRadar.Business.Entities.LeagueEntities
{
    public class Standing
    {
        public Guid Id { get; set; }
        public int Season { get; set; }
        public int Rank { get; set; }
        public Guid TeamId { get; set; }
        public Guid LeagueId { get; set; }
        public int Points { get; set; }
        public int GoalsDiff { get; set; }
        public Guid StandingStatsId { get; set; }
    }
}
