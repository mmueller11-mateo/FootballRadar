using FootballRadar.Business.Entities.TeamEntities;
using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public sealed class GetAllTeamsQuery : IRequest<IEnumerable<Team>>
    {
    }
}
