using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Entities.LeagueEntities;

namespace FootballRadar.Business.Services.MatchPredictionMarketRules
{
    abstract class MatchPredictionMarketRule : IPredictionMarketRule
    {
        protected MatchPredictionMarketRule(Match match)
        {
            this.Match = match;
        }

        public Match Match { get; }

        public abstract string ErrorMessage { get; }

        public abstract Task<bool> Evaluate();
    }
}
