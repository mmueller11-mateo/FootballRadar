namespace FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Standing
{
    public class Standing
    {
        public int Rank { get; set; }
        public required StandingTeam Team { get; set; }
        public int Points { get; set; }
        public int GoalsDiff { get; set; }
        public required string Description { get; set; }
        public required StandingStats All { get; set; }
    }
}
