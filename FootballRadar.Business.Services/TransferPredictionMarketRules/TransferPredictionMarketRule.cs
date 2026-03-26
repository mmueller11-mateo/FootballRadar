using FootballRadar.Business.Entities.Betting;

namespace FootballRadar.Business.Services.TransferPredictionMarketRules
{
    abstract class TransferPredictionMarketRule : IPredictionMarketRule
    {
        protected TransferPredictionMarketRule(TransferPredictionMarket market)
        {
            this.Market = market;
        }

        public TransferPredictionMarket Market { get; }
        public abstract string ErrorMessage { get; }
        public abstract Task<bool> Evaluate();
    }
}
