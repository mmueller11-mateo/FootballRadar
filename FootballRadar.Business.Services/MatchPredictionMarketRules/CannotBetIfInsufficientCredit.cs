
using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Entities.LeagueEntities;

namespace FootballRadar.Business.Services.MatchPredictionMarketRules
{
    internal sealed class CannotBetIfInsufficientCredit : MatchPredictionMarketRule
    {
        private readonly Wallet wallet;
        private readonly decimal betAmount;
        private readonly Match match;

        public CannotBetIfInsufficientCredit(Wallet wallet, decimal betAmount, Match match) : base(match)
        {
            this.wallet = wallet;
            this.betAmount = betAmount;
            this.match = match;
        }

        public override string ErrorMessage => "Insufficient credits.";

        public override Task<bool> Evaluate()
        {
            return Task.FromResult(wallet.Credits >= betAmount);
        }
    }
}
