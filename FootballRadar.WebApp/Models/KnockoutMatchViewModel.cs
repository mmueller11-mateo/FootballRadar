namespace FootballRadar.WebApp.Models
{
    public class KnockoutMatchViewModel
    {
        public Guid Id { get; set; }
        public string Round { get; set; } = "";

        public string? HomeTeam { get; set; }
        public string? AwayTeam { get; set; }

        public string? HomeSource { get; set; }   // A1, B2, W49...
        public string? AwaySource { get; set; }

        public int? HomeGoals { get; set; }
        public int? AwayGoals { get; set; }

        public DateTimeOffset KickoffUtc { get; set; }
    }
}
