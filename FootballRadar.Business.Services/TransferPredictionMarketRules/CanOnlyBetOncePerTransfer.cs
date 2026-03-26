using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;

namespace FootballRadar.Business.Services.TransferPredictionMarketRules
{
    sealed class CanOnlyBetOncePerTransfer : TransferPredictionMarketRule
    {
        private readonly Guid _userId;
        private readonly IBetRepository _betRepository;

        public CanOnlyBetOncePerTransfer(TransferPredictionMarket market, Guid userId, IBetRepository betRepository) : base(market)
        {
            _userId = userId;
            _betRepository = betRepository;
        }

        public override async Task<bool> Evaluate()
        {
            return !await _betRepository.HasUserBetOnMarketAsync(_userId, this.Market.Id);
        }

        public override string ErrorMessage { get; } = "You can only place one bet per transfer";
    }
}
