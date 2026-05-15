using FootballRadar.Abstractions;

namespace FootballRadar.Business.Services.MatchPredictionMarketRules
{
    sealed class CannotBetAfterMatchStart : MatchPredictionMarketRule
    {
        private readonly IMatchRepository matchRepository;
        public CannotBetAfterMatchStart(MatchPredictionContext context, IMatchRepository matchRepository) : base(context)
        {
            this.matchRepository = matchRepository;
        }

        public override async Task<bool> Evaluate(CancellationToken cancellationToken)
        {
            var match = await this.matchRepository.GetByIdAsync(Context.MatchId, cancellationToken);
            return match.Date <= DateTimeOffset.UtcNow;
        }

        public override string ErrorMessage { get; } = "Betting is not allowed after match start";
    }
}
