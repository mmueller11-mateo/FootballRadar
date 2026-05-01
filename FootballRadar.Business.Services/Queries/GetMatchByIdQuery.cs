using FootballRadar.Business.Entities.LeagueEntities;
using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public sealed class GetMatchByIdQuery : IRequest<Match?>
    {
        public required Guid MatchId { get; init; }
    }
}