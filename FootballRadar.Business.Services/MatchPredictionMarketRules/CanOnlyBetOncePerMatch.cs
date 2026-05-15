using FootballRadar.Abstractions;

namespace FootballRadar.Business.Services.MatchPredictionMarketRules
{
    sealed class CanOnlyBetOncePerMatch : MatchPredictionMarketRule
    {
        private readonly IBetRepository _betRepository;

        public CanOnlyBetOncePerMatch(MatchPredictionContext context, IBetRepository betRepository) : base(context)
        {
            _betRepository = betRepository;
        }

        public override async Task<bool> Evaluate(CancellationToken cancellationToken)
        {
            return !await _betRepository.HasUserBetOnMatchAsync(Context.UserId, Context.MatchId);
        }

        public override string ErrorMessage { get; } = "You can only place one bet per match";
    }
}