
using FootballRadar.Abstractions;

namespace FootballRadar.Business.Services.MatchPredictionMarketRules
{
    internal sealed class CannotBetIfInsufficientCredit : MatchPredictionMarketRule
    {
        private readonly IWalletRepository walletRepository;

        public CannotBetIfInsufficientCredit(MatchPredictionContext context, IWalletRepository walletRepository) : base(context)
        {
            this.walletRepository = walletRepository;
        }

        public override string ErrorMessage => "Insufficient credits.";

        public override async Task<bool> Evaluate(CancellationToken cancellationToken)
        {
            var wallet = await this.walletRepository.GetByUserIdAsync(Context.UserId, cancellationToken);
            return wallet.Credits >= Context.Credits;
        }
    }
}
