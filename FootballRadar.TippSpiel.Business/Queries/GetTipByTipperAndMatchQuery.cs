using FootballRadar.Business.Entities.TippSpiel;
using MediatR;

namespace FootballRadar.TippSpiel.Business.Queries
{
    public class GetTipByTipperAndMatchQuery : IRequest<Tip?>
    {
        public Guid TipperId { get; set; }
        public Guid MatchId { get; set; }
    }
}
