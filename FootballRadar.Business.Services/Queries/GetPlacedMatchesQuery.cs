using FootballRadar.Business.Entities.LeagueEntities;
using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public class GetPlayedMatchesQuery : IRequest<IEnumerable<Match>>
    {
    }
}
