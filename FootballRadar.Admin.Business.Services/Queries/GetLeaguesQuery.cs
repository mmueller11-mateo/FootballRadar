using FootballRadar.Business.Entities.LeagueEntities;
using MediatR;

namespace FootballRadar.Admin.Business.Services.Queries
{
    public sealed class GetLeaguesQuery : IRequest<IEnumerable<PublicLeague>>
    {
    }
}
