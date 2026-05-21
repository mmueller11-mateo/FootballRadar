using FootballRadar.Business.Entities.TippSpiel;
using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public class GetTipsByMatchQuery : IRequest<IEnumerable<WmTip>>
    {
        public Guid MatchId { get; set; }
    }
}
