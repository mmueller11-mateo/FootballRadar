using FootballRadar.Business.Entities.Betting;

namespace FootballRadar.Business.Services.MatchPredictionMarketRules
{
    abstract class MatchPredictionMarketRule : IPredictionMarketRule
    {
        protected MatchPredictionMarketRule(MatchPredictionContext context)
        {
            Context = context;
        }

        public abstract string ErrorMessage { get; }
        protected MatchPredictionContext Context { get; }

        public abstract Task<bool> Evaluate(CancellationToken cancellationToken);
    }
}
