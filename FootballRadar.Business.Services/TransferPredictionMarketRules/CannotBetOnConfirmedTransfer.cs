using FootballRadar.Business.Entities.Betting;

namespace FootballRadar.Business.Services.TransferPredictionMarketRules
{
    sealed class CannotBetOnConfirmedTransfer : TransferPredictionMarketRule
    {
        public CannotBetOnConfirmedTransfer(TransferPredictionMarket market) : base(market) { }

        public override Task<bool> Evaluate()
        {
            return Task.FromResult(!this.Market.IsConfirmed);
        }

        public override string ErrorMessage { get; } = "Betting is not allowed on a confirmed transfer";
    }
}
