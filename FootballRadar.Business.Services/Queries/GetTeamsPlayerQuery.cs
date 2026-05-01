using FootballRadar.Business.Entities.PlayerIEntities;
using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public sealed class GetTeamPlayersQuery : IRequest<IEnumerable<Player>>
    {
        public required int ApiTeamId { get; init; }
        public required int Season { get; init; }
    }
}
