using FootballRadar.Business.Entities.LeagueEntities;

namespace FootballRadar.Business.Services.MatchPredictionMarketRules
{
    sealed class CannotBetAfterMatchEnd : MatchPredictionMarketRule
    {
        public CannotBetAfterMatchEnd(Match match) : base(match) { }

        public override Task<bool> Evaluate()
        {
            return Task.FromResult(this.Match.Date > DateTimeOffset.UtcNow);
        }

        public override string ErrorMessage { get; } = "Betting is not allowed after match end";
    }
}