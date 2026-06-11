using FootballRadar.Business.Entities.Betting.Enums;
using FootballRadar.Business.Entities.Enums;

namespace FootballRadar.Business.Services.MatchPredictionMarketRules
{
    sealed class CannotBetDrawInKnockout : MatchPredictionMarketRule
    {
        private readonly WmPhase? _wmPhase;

        public CannotBetDrawInKnockout(MatchPredictionContext context, WmPhase? wmPhase) : base(context)
        {
            _wmPhase = wmPhase;
        }

        public override Task<bool> Evaluate(CancellationToken cancellationToken)
        {
            // If it's a knockout match (phase not Group), draws are not allowed
            if (_wmPhase != null && _wmPhase != WmPhase.Group && Context.Prediction == MatchPrediction.Draw)
                return Task.FromResult(false);

            return Task.FromResult(true);
        }

        public override string ErrorMessage { get; } = "Draw predictions are not allowed for knockout matches";
    }
}
