using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Entities.Betting.Enums;
using MediatR;

namespace FootballRadar.Business.Services.Commands
{
    public sealed class PlaceTransferBetCommand : IRequest<BetStatus>
    {
        public required Guid UserId { get; init; }
        public required Guid TransferRumorId { get; init; }
        public required decimal Credits { get; init; }
        public required TransferPrediction Prediction { get; init; }
    }
}
