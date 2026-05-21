namespace FootballRadar.Business.Services.MatchPredictionMarketRules
{
    sealed class CannotBetAfterMatchEnd : MatchPredictionMarketRule
    {
        private readonly string? _matchStatus;

        public CannotBetAfterMatchEnd(MatchPredictionContext context, string? matchStatus) : base(context)
        {
            _matchStatus = matchStatus;
        }

        public override Task<bool> Evaluate(CancellationToken cancellationToken)
            => Task.FromResult(_matchStatus != "FT");

        public override string ErrorMessage { get; } = "Betting is not allowed after match end";
    }
}