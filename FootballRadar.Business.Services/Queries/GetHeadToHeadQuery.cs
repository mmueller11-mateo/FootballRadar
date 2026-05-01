using FootballRadar.Business.Entities.LeagueEntities;
using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public sealed class GetHeadToHeadQuery : IRequest<IEnumerable<Match>>
    {
        public required Guid HomeTeamId { get; init; }
        public required Guid AwayTeamId { get; init; }
        public int Limit { get; init; } = 5;
    }
}