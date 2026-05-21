using FootballRadar.Business.Entities.Betting;

namespace FootballRadar.Business.Services.MatchPredictionMarketRules
{
    internal interface IBetRuleFactory
    {
        Task<IEnumerable<IPredictionMarketRule>> CreateRulesAsync(MatchPredictionContext context, CancellationToken cancellationToken);
    }
}