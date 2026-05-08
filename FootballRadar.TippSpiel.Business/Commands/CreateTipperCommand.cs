using FootballRadar.Business.Entities.TippSpiel;
using MediatR;

namespace FootballRadar.TippSpiel.Business.Commands
{
    public class CreateTipperCommand : IRequest<Tipper>
    {
        public string Name { get; set; } = string.Empty;
    }
}
