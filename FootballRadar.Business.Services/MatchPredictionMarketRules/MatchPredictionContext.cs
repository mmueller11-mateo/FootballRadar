using FootballRadar.Business.Entities.Betting.Enums;

namespace FootballRadar.Business.Services.MatchPredictionMarketRules
{
    internal sealed class MatchPredictionContext
    {
        public required Guid UserId { get; init; }
        public required Guid MatchId { get; init; }
        public required decimal Credits { get; init; }
        public required MatchPrediction Prediction { get; init; }
    }
}
