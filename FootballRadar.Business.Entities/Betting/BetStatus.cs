using FootballRadar.Business.Entities.Betting.Enums;

namespace FootballRadar.Business.Entities.Betting
{
    public sealed class BetStatus
    {
        public BetStatusCode Code { get; set; }
        public string? ErrorMessage { get; set; }
    }
}