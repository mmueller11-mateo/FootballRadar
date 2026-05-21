namespace FootballRadar.Business.Services.MatchPredictionMarketRules
{
    internal sealed class CannotBetIfInsufficientCredit : MatchPredictionMarketRule
    {
        private readonly decimal _availableCredits;

        public CannotBetIfInsufficientCredit(MatchPredictionContext context, decimal availableCredits) : base(context)
        {
            _availableCredits = availableCredits;
        }

        public override string ErrorMessage => "Insufficient credits.";

        public override Task<bool> Evaluate(CancellationToken cancellationToken)
            => Task.FromResult(_availableCredits >= Context.Credits);
    }
}