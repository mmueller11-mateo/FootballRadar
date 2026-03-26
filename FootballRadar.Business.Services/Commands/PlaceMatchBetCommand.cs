using FootballRadar.Business.Entities;
using FootballRadar.Business.Entities.Betting;
using MediatR;

namespace FootballRadar.Business.Services.Commands
{
    public sealed class PlaceMatchBetCommand : IRequest<BetStatus>
    {
        public required Guid UserId { get; init; }
        public required Guid MatchId { get; init; }
        public required int HomeTeamScore { get; init; }
        public required int AwayTeamScore { get; init; }
        public required Money Amount { get; init; }
    }
}
