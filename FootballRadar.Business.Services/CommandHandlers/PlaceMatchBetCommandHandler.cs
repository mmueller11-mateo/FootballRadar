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

        public PlaceMatchBetCommandHandler(IBetRepository betRepository, IPredictionMarketRepository predictionMarketRepository, IMatchRepository matchRepository, IMatchPredictionRewardCalculator rewardCalculator)
        {
            _betRepository = betRepository;
            _predictionMarketRepository = predictionMarketRepository;
            _matchRepository = matchRepository;
            _rewardCalculator = rewardCalculator;
        }

        public async Task<BetStatus> Handle(PlaceMatchBetCommand request, CancellationToken cancellationToken)
        {
            var match = await _matchRepository.GetByIdAsync(request.MatchId);
            if (match == null)
            {
                throw new InvalidOperationException("Match not found");
            }

            var predictionMarket = await _predictionMarketRepository.FindForMatchAsync(request.MatchId);
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
                        new CanOnlyBetOncePerMatch(match, request.UserId, _betRepository)]
                };

                await _predictionMarketRepository.AddAsync(predictionMarket);
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

            var bet = new Bet
            {
                Id = Guid.NewGuid(),
                Amount = request.Amount,
                PlacedAt = DateTimeOffset.UtcNow,
                UserId = request.UserId,
                PredictionMarketId = predictionMarket.Id,
            };

            await _betRepository.AddBetAsync(bet);
            return new BetStatus { Code = BetStatusCode.Accepted };
        }
    }
}