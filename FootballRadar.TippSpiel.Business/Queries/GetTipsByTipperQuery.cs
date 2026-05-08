using FootballRadar.Business.Entities.TippSpiel;
using MediatR;

namespace FootballRadar.TippSpiel.Business.Queries
{
    public class GetTipsByTipperQuery : IRequest<IEnumerable<Tip>>
    {
        public Guid TipperId { get; set; }
    }
}
