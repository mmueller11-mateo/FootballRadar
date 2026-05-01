using FootballRadar.Business.Entities.Enums;
using FootballRadar.Business.Entities.TransferEntities;
using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public sealed class GetTransferRumorsQuery : IRequest<IEnumerable<TransferRumor>>
    {
        public required RumorStatus RumorStatus { get; init; }
    }
}
