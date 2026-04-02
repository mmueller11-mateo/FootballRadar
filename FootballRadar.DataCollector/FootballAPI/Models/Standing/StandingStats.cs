namespace FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Standing
{
    public class StandingStats
    {
        public int Played { get; set; }
        public int Win { get; set; }
        public int Draw { get; set; }
        public int Lose { get; set; }
        public required StandingGoals Goals { get; set; }
    }
}
