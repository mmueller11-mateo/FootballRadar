using FootballRadar.Business.Entities.TippSpiel;
using MediatR;

namespace FootballRadar.TippSpiel.Business.Queries
{
    public class GetPlayedMatchesQuery : IRequest<IEnumerable<TippMatch>>
    {
    }
}
