using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Entities.Betting.Enums;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FootballRadar.Business.Services.Background
{
    public sealed class BetSettlementWorker : BackgroundService
    {
        private readonly ILogger<BetSettlementWorker> logger;
        private readonly IMatchRepository matchRepository;
        private readonly IBetRepository betRepository;
        private readonly IWalletRepository walletRepository;
        private readonly IPredictionMarketRepository predictionMarketRepository;

        public BetSettlementWorker(ILogger<BetSettlementWorker> logger, IMatchRepository matchRepository, IBetRepository betRepository, IWalletRepository walletRepository, IPredictionMarketRepository predictionMarketRepository)
        {
            this.logger = logger;
            this.matchRepository = matchRepository;
            this.betRepository = betRepository;
            this.walletRepository = walletRepository;
            this.predictionMarketRepository = predictionMarketRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await SettleAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task SettleAsync(CancellationToken cancellationToken)
        {
            var finishedMatches = (await matchRepository.GetUpcomingMatches(cancellationToken))
                .Where(m => m.Status == "FT" && m.HomeGoals.HasValue && m.AwayGoals.HasValue);

            foreach (var match in finishedMatches)
            {
                var market = await predictionMarketRepository.FindForMatchAsync(match.Id, cancellationToken);
                if (market == null) continue;
                if (market is not MatchPredictionMarket matchMarket) continue;
                if (matchMarket.IsSettled) continue;

                var bets = await betRepository.GetMatchBetsByMarketIdAsync(market.Id, cancellationToken);
                if (!bets.Any()) continue;

                MatchPrediction correctPrediction;
                if (match.HomeGoals > match.AwayGoals)
                    correctPrediction = MatchPrediction.HomeWin;
                else if (match.AwayGoals > match.HomeGoals)
                    correctPrediction = MatchPrediction.AwayWin;
                else
                    correctPrediction = MatchPrediction.Draw;

                foreach (var bet in bets)
                {
                    if (bet.Prediction != correctPrediction) continue;

                    var wallet = await walletRepository.GetByUserIdAsync(bet.UserId, cancellationToken);
                    if (wallet == null) continue;

                    var payout = bet.Credits * (1 + matchMarket.Reward / 100m);
                    wallet.Deposit(payout);
                    await walletRepository.UpdateAsync(wallet, cancellationToken);

                    logger.LogInformation("Settled bet {BetId} for user {UserId}: +{Payout} credits", bet.Id, bet.UserId, payout);
                }

                matchMarket.IsSettled = true;
                await predictionMarketRepository.UpdateAsync(matchMarket, cancellationToken);
            }
        }
    }
}