namespace FootballRadar.Business.ViewModels
{
    public class HeadToHeadViewModel
    {
        public DateTimeOffset Date { get; set; }
        public string HomeTeam { get; set; } = string.Empty;
        public string AwayTeam { get; set; } = string.Empty;
        public int? HomeGoals { get; set; }
        public int? AwayGoals { get; set; }
    }
}
