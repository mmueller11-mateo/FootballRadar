namespace FootballRadar.DataCollector.FootballAPI.Models.Standing
{
    public class StandingStats
    {
        public int Played { get; set; }
        public int Win { get; set; }
        public int Draw { get; set; }
        public int Lose { get; set; }
        public StandingGoals Goals { get; set; }
    }
}
