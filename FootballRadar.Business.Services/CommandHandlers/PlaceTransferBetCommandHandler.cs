using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Entities.Betting.Enums;
using FootballRadar.Business.Services.Commands;
using FootballRadar.Business.Services.TransferPredictionMarketRules;
using MediatR;

namespace FootballRadar.Business.Services.CommandHandlers
{
    internal sealed class PlaceTransferBetCommandHandler : IRequestHandler<PlaceTransferBetCommand, BetStatus>
    {
        private readonly ITransferRepository _transferRepository;
        private readonly IPredictionMarketRepository _predictionMarketRepository;
        private readonly IBetRepository _betRepository;
        private readonly ITransferPredictionRewardCalculator _rewardCalculator;


        public PlaceTransferBetCommandHandler(ITransferRepository transferRepository, IPredictionMarketRepository predictionMarketRepository, IBetRepository betRepository, ITransferPredictionRewardCalculator transferPredictionRewardCalculator)
        {
            this._transferRepository = transferRepository;
            this._predictionMarketRepository = predictionMarketRepository;
            this._betRepository = betRepository;
            this._rewardCalculator = transferPredictionRewardCalculator;
        }

        public async Task<BetStatus> Handle(PlaceTransferBetCommand request, CancellationToken cancellationToken)
        {
            var rumor = await _transferRepository.GetTransferRumorById(request.TransferRumorId);
            if (rumor is null)
            {
                throw new InvalidOperationException("Transfer rumor not found");
            }

            var predictionMarket = await _predictionMarketRepository.FindForTransferRumorAsync(request.TransferRumorId);
            if (predictionMarket is null)
            {
                var reward = await _rewardCalculator.CalculateReward(rumor);
                var transferMarket = new TransferPredictionMarket
                {
                    Id = Guid.NewGuid(),
                    Title = "Transfer Rumor",
                    StartTime = DateTimeOffset.UtcNow,
                    EndTime = DateTimeOffset.UtcNow.AddDays(30),
                    Reward = reward,
                    TransferRumorId = request.TransferRumorId,
                };
                transferMarket.Rules =
                [
                    new CanOnlyBetOncePerTransfer(transferMarket, request.UserId, _betRepository),
                    new CannotBetAfterTransferDeadline(transferMarket)
                ];
                predictionMarket = transferMarket;
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
