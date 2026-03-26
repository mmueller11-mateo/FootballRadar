namespace FootballRadar.DataCollector.FootballAPI.Models.Standing
{
    public class Standing
    {
        public int Rank { get; set; }
        public StandingTeam Team { get; set; }
        public int Points { get; set; }
        public int GoalsDiff { get; set; }
        public string Description { get; set; }
        public StandingStats All { get; set; }
    }
}
