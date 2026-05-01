
using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public sealed class GetTeamSeasonsQuery : IRequest<IEnumerable<int>>
    {
        public required int ApiTeamId { get; init; }
    }
}

