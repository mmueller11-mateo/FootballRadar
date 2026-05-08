using FootballRadar.Business.Entities.TippSpiel;
using MediatR;

namespace FootballRadar.TippSpiel.Business.Queries
{
    public class GetTipsByMatchQuery : IRequest<IEnumerable<Tip>>
    {
        public Guid MatchId { get; set; }
    }
}
