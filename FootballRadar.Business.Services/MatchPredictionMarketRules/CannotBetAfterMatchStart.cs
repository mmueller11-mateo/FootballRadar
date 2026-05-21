namespace FootballRadar.Business.Services.MatchPredictionMarketRules
{
    sealed class CannotBetAfterMatchStart : MatchPredictionMarketRule
    {
        private readonly DateTimeOffset _matchStartTime;

        public CannotBetAfterMatchStart(MatchPredictionContext context, DateTimeOffset matchStartTime) : base(context)
        {
            _matchStartTime = matchStartTime;
        }

        public override Task<bool> Evaluate(CancellationToken cancellationToken)
            => Task.FromResult(DateTimeOffset.UtcNow < _matchStartTime);

        public override string ErrorMessage { get; } = "Betting is not allowed after match start";
    }
}