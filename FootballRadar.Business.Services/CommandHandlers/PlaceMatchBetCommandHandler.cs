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
        private readonly IBetRepository _betRepository;
        private readonly IPredictionMarketRepository _predictionMarketRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IMatchPredictionRewardCalculator _rewardCalculator;
        private readonly IWalletRepository _walletRepository;

        public PlaceMatchBetCommandHandler(IBetRepository betRepository, IPredictionMarketRepository predictionMarketRepository, IMatchRepository matchRepository, IMatchPredictionRewardCalculator rewardCalculator, IWalletRepository walletRepository)
        {
            _betRepository = betRepository;
            _predictionMarketRepository = predictionMarketRepository;
            _matchRepository = matchRepository;
            _rewardCalculator = rewardCalculator;
            _walletRepository = walletRepository;
        }

        public async Task<BetStatus> Handle(PlaceMatchBetCommand request, CancellationToken cancellationToken)
        {
            var match = await _matchRepository.GetByIdAsync(request.MatchId, cancellationToken);
            if (match == null)
            {
                throw new InvalidOperationException("Match not found");
            }

            var predictionMarket = await _predictionMarketRepository.FindForMatchAsync(request.MatchId, cancellationToken);
            if (predictionMarket == null)
            {
                var reward = await _rewardCalculator.CalculateReward(match);
                predictionMarket = new MatchPredictionMarket
                {
                    Id = Guid.NewGuid(),
                    StartTime = DateTimeOffset.UtcNow,
                    EndTime = match.Date,
                    Title = "To be defined",
                    Reward = reward,
                    MatchId = request.MatchId,
                    Rules = [
                        new CannotBetAfterMatchStart(match),
                        new CannotBetAfterMatchEnd(match),
                        new CanOnlyBetOncePerMatch(match, request.UserId, _betRepository)
                    ]
                };
                await _predictionMarketRepository.AddAsync(predictionMarket, cancellationToken);
            }
            else
            {
                predictionMarket.Rules = [
                    new CannotBetAfterMatchStart(match),
                    new CannotBetAfterMatchEnd(match),
                    new CanOnlyBetOncePerMatch(match, request.UserId, _betRepository)
                ];
            }

            foreach (var rule in predictionMarket.Rules)
            {
                if (!await rule.Evaluate())
                {
                    return new BetStatus
                    {
                        Code = BetStatusCode.Rejected,
                        ErrorMessage = rule.ErrorMessage
                    };
                }
            }

            var wallet = await _walletRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (wallet == null)
            {
                return new BetStatus
                {
                    Code = BetStatusCode.Rejected,
                    ErrorMessage = "Wallet not found."
                };
            }

            if (wallet.Credits < request.Credits)
            {
                return new BetStatus
                {
                    Code = BetStatusCode.Rejected,
                    ErrorMessage = "Insufficient credits."
                };
            }

            wallet.Withdraw(request.Credits);
            await _walletRepository.UpdateAsync(wallet, cancellationToken);

            var bet = new MatchBet
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