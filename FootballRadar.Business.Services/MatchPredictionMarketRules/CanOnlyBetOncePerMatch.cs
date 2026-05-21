namespace FootballRadar.Business.Services.MatchPredictionMarketRules
{
    sealed class CanOnlyBetOncePerMatch : MatchPredictionMarketRule
    {
        private readonly bool _hasAlreadyBet;

        public CanOnlyBetOncePerMatch(MatchPredictionContext context, bool hasAlreadyBet) : base(context)
        {
            _hasAlreadyBet = hasAlreadyBet;
        }

        public override Task<bool> Evaluate(CancellationToken cancellationToken)
            => Task.FromResult(!_hasAlreadyBet);

        public override string ErrorMessage { get; } = "You can only place one bet per match";
    }
}