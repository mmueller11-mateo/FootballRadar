using FootballRadar.Business.Entities.Betting;

namespace FootballRadar.Business.Services.TransferPredictionMarketRules
{
    sealed class CannotBetAfterTransferDeadline : TransferPredictionMarketRule
    {
        public CannotBetAfterTransferDeadline(TransferPredictionMarket market) : base(market) { }

        public override Task<bool> Evaluate(CancellationToken cancellationToken)
        {
            return Task.FromResult(this.Market.EndTime > DateTimeOffset.UtcNow);
        }

        public override string ErrorMessage { get; } = "Betting is not allowed after transfer deadline";
    }
}
