using FootballRadar.Business.Entities.TippSpiel;
using MediatR;

namespace FootballRadar.TippSpiel.Business.Queries
{
    public class GetMatchesQuery : IRequest<IEnumerable<TippMatch>>
    {
    }
}
