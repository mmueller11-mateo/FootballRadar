using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Entities.Betting.Enums;
using MediatR;

namespace FootballRadar.Business.Services.Commands
{
    public sealed class PlaceMatchBetCommand : IRequest<BetStatus>
    {
        public required Guid UserId { get; init; }
        public required Guid MatchId { get; init; }
        public required decimal Credits { get; init; }
        public required MatchPrediction Prediction { get; init; }
    }
}
