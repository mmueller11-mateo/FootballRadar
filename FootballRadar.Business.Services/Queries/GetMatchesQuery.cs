using FootballRadar.Business.Entities.LeagueEntities;
using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public class GetMatchesQuery : IRequest<IEnumerable<Match>>
    {
    }
}
