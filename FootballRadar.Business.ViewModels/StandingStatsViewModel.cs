namespace FootballRadar.Business.ViewModels
{
    public class StandingStatsViewModel
    {
        public int Played { get; set; }
        public int Win { get; set; }
        public int Draw { get; set; }
        public int Lose { get; set; }
        public required StandingGoalsViewModel Goals { get; set; }
    }
}
