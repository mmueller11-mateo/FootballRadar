using FootballRadar.Business.Entities.LeagueEntities;

namespace FootballRadar.Business.Services.MatchPredictionMarketRules
{
    sealed class CannotBetAfterMatchStart : MatchPredictionMarketRule
    {
        public CannotBetAfterMatchStart(Match match) : base(match) { }

        public override Task<bool> Evaluate()
        {
            return Task.FromResult(this.Match.Date > DateTimeOffset.UtcNow);
        }

        public override string ErrorMessage { get; } = "Betting is not allowed after match start";
    }
}
