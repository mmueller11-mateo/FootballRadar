using FootballRadar.Business.Entities.LeagueEntities;
using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public sealed class GetMatchesBySeasonQuery : IRequest<IEnumerable<Match>>
    {
        public required int ApiLeagueId { get; init; }
        public required int Season { get; init; }
    }
}
