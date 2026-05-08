using FootballRadar.Business.Entities.TippSpiel;
using MediatR;

namespace FootballRadar.TippSpiel.Business.Queries
{
    public class GetTipperByNameQuery : IRequest<Tipper?>
    {
        public string Name { get; set; } = string.Empty;
    }
}
