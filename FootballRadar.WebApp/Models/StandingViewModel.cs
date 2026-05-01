namespace FootballRadar.WebApp.Models
{
    public class StandingViewModel
    {
        public int Rank { get; set; }
        public int Points { get; set; }
        public int GoalsDiff { get; set; }
        public string? Description { get; set; }
        public required StandingTeamViewModel Team { get; set; }
        public required StandingStatsViewModel All { get; set; }
    }
}