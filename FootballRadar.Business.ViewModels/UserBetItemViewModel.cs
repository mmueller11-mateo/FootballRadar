using FootballRadar.Business.Entities.Betting.Enums;

namespace FootballRadar.Business.ViewModels
{
    public class UserBetItemViewModel
    {
        public Guid BetId { get; set; }
        public string HomeTeam { get; set; } = string.Empty;
        public string AwayTeam { get; set; } = string.Empty;
        public string? HomeLogo { get; set; }
        public string? AwayLogo { get; set; }
        public DateTimeOffset MatchDate { get; set; }
        public MatchPrediction Prediction { get; set; }
        public decimal Credits { get; set; }
        public DateTimeOffset PlacedAt { get; set; }
        // Settled
        public bool IsSettled { get; set; }
        public bool? IsWon { get; set; }
        public int? HomeGoals { get; set; }
        public int? AwayGoals { get; set; }
        public decimal? Payout { get; set; }
        public decimal Reward { get; set; }
    }
}
