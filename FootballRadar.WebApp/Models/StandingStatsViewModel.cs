namespace FootballRadar.WebApp.Models
{
    public class StandingStatsViewModel
    {
        public int Played { get; set; }
        public int Win { get; set; }
        public int Draw { get; set; }
        public int Lose { get; set; }
        public StandingGoalsViewModel Goals { get; set; }
    }
}
