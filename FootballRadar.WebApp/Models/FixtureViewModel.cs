using System.ComponentModel.DataAnnotations;

namespace FootballRadar.WebApp.Models
{
    public class FixtureViewModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public required string Round { get; set; }
        public required string Status { get; set; }
        public required string HomeTeam { get; set; }
        public required string AwayTeam { get; set; }
        [Required]
        public string HomeLogo { get; set; } = string.Empty;
        [Required]
        public string AwayLogo { get; set; } = string.Empty;
        public int? HomeGoals { get; set; }
        public int? AwayGoals { get; set; }
    }
}
