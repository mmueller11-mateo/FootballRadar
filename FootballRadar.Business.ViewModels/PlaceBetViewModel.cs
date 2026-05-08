using FootballRadar.Business.Entities.Betting.Enums;
using System.ComponentModel.DataAnnotations;

namespace FootballRadar.Business.ViewModels
{
    public class PlaceBetViewModel
    {
        public Guid MatchId { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Credits { get; set; }

        [Required]
        public MatchPrediction Prediction { get; set; }

        // Match Info
        public string HomeTeam { get; set; } = string.Empty;
        public string AwayTeam { get; set; } = string.Empty;
        public string? HomeLogo { get; set; }
        public string? AwayLogo { get; set; }
        public DateTimeOffset MatchDate { get; set; }
        public string? Round { get; set; }

        // User Info
        public decimal AvailableCredits { get; set; }

        // Head to Head
        public IEnumerable<HeadToHeadViewModel> HeadToHead { get; set; } = new List<HeadToHeadViewModel>();
    }


}
