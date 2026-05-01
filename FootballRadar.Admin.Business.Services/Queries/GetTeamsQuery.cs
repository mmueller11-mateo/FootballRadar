using FootballRadar.Business.Entities.TeamEntities;
using MediatR;

namespace FootballRadar.Admin.Business.Services.Queries
{
    public sealed class GetTeamsQuery : IRequest<IEnumerable<Team>>
    {
    }
}
