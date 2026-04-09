using FootballRadar.Business.Entities.PlayerIEntities;
using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public sealed class GetTeamPlayersQuery : IRequest<IReadOnlyCollection<Player>>
    {
        public required int ApiTeamId { get; init; }
    }
}
