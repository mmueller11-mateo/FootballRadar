using FootballRadar.Business.Entities.TippSpiel;
using MediatR;

namespace FootballRadar.TippSpiel.Business.Queries
{
    public class GetMatchByIdQuery : IRequest<TippMatch?>
    {
        public Guid MatchId { get; set; }
    }
}
