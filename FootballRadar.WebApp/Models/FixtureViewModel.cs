namespace FootballRadar.WebApp.Models
{
    public class FixtureViewModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Round { get; set; }
        public string Status { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string? HomeLogo { get; set; }
        public string? AwayLogo { get; set; }
        public int? HomeGoals { get; set; }
        public int? AwayGoals { get; set; }
    }
}
