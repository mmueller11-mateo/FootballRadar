using FootballRadar.Business.Entities.TeamEntities;
using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public sealed class GetTeamsQuery : IRequest<IEnumerable<Team>>
    {
        public required int Season { get; init; }
    }
}
