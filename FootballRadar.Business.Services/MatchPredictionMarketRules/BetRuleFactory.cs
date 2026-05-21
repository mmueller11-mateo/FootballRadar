using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;

namespace FootballRadar.Business.Services.MatchPredictionMarketRules
{
    internal sealed class BetRuleFactory : IBetRuleFactory
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IBetRepository _betRepository;
        private readonly IWalletRepository _walletRepository;

        public BetRuleFactory(IMatchRepository matchRepository, IBetRepository betRepository, IWalletRepository walletRepository)
        {
            _matchRepository = matchRepository;
            _betRepository = betRepository;
            _walletRepository = walletRepository;
        }

        public async Task<IEnumerable<IPredictionMarketRule>> CreateRulesAsync(
            MatchPredictionContext context, CancellationToken cancellationToken)
        {
            var match = await _matchRepository.GetByIdAsync(context.MatchId, cancellationToken)
                ?? throw new InvalidOperationException("Match not found");

            var wallet = await _walletRepository.GetByUserIdAsync(context.UserId, cancellationToken)
                ?? throw new InvalidOperationException("Wallet not found for user");

            var hasAlreadyBet = await _betRepository.HasUserBetOnMatchAsync(context.UserId, context.MatchId, cancellationToken);

            return [
     new CannotBetAfterMatchStart(context, match.Date),
    new CannotBetAfterMatchEnd(context, match.Status),
    new CanOnlyBetOncePerMatch(context, hasAlreadyBet),
    new CannotBetIfInsufficientCredit(context, wallet.Credits)
 ];
        }
    }
}