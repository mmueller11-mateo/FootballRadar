using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Entities.Betting.Enums;
using FootballRadar.Business.Services.Commands;
using FootballRadar.Business.Services.MatchPredictionMarketRules;
using MediatR;

namespace FootballRadar.Business.Services.CommandHandlers
{
    internal sealed class PlaceMatchBetCommandHandler : IRequestHandler<PlaceMatchBetCommand, BetStatus>
    {
        private readonly IPredictionMarketRepository _predictionMarketRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IMatchPredictionRewardCalculator _rewardCalculator;
        private readonly IWalletRepository _walletRepository;
        private readonly IBetRepository _betRepository;
        private readonly IBetRuleFactory _ruleFactory;

        public PlaceMatchBetCommandHandler(
            IPredictionMarketRepository predictionMarketRepository,
            IMatchRepository matchRepository,
            IMatchPredictionRewardCalculator rewardCalculator,
            IWalletRepository walletRepository,
            IBetRepository betRepository,
            IBetRuleFactory ruleFactory)
        {
            _predictionMarketRepository = predictionMarketRepository;
            _matchRepository = matchRepository;
            _rewardCalculator = rewardCalculator;
            _walletRepository = walletRepository;
            _betRepository = betRepository;
            _ruleFactory = ruleFactory;
        }

        public async Task<BetStatus> Handle(PlaceMatchBetCommand request, CancellationToken cancellationToken)
        {
            var match = await _matchRepository.GetByIdAsync(request.MatchId, cancellationToken)
                ?? throw new InvalidOperationException("Match not found");

            var context = new MatchPredictionContext
            {
                Credits = request.Credits,
                MatchId = request.MatchId,
                Prediction = request.Prediction,
                UserId = request.UserId
            };

            var predictionMarket = await _predictionMarketRepository.FindForMatchAsync(request.MatchId, cancellationToken);
            if (predictionMarket == null)
            {
                var reward = await _rewardCalculator.CalculateReward(match);
                var rules = await _ruleFactory.CreateRulesAsync(context, cancellationToken);
                predictionMarket = new MatchPredictionMarket
                {
                    Id = Guid.NewGuid(),
                    StartTime = DateTimeOffset.UtcNow,
                    EndTime = match.Date,
                    Title = "To be defined",
                    Reward = reward,
                    MatchId = request.MatchId,
                    Rules = rules.ToList()
                };
                await _predictionMarketRepository.AddAsync(predictionMarket, cancellationToken);
            }

            foreach (var rule in predictionMarket.Rules)
            {
                if (!await rule.Evaluate(cancellationToken))
                {
                    return new BetStatus
                    {
                        Code = BetStatusCode.Rejected,
                        ErrorMessage = rule.ErrorMessage
                    };
                }
            }

            var wallet = await _walletRepository.GetByUserIdAsync(request.UserId, cancellationToken)
                ?? throw new InvalidOperationException("Wallet not found for user");

            wallet.Withdraw(request.Credits);
            await _walletRepository.UpdateAsync(wallet, cancellationToken);

            var bet = new WinnerBet
            {
                Id = Guid.NewGuid(),
                Credits = request.Credits,
                PlacedAt = DateTimeOffset.UtcNow,
                UserId = request.UserId,
                PredictionMarketId = predictionMarket.Id,
                Prediction = request.Prediction
            };

            await _betRepository.AddBetAsync(bet, cancellationToken);
            return new BetStatus { Code = BetStatusCode.Accepted };
        }
    }
}