using FootballRadar.Business.Entities.TeamEntities;
using MediatR;

namespace FootballRadar.Admin.Business.Services.Queries
{
    public sealed class GetNationalTeamsQuery : IRequest<IEnumerable<NationalTeam>>
    {
    }
}
