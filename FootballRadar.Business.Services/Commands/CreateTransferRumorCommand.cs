using FootballRadar.Business.Entities.Enums;
using FootballRadar.Business.Entities.TransferEntities;
using MediatR;

namespace FootballRadar.Business.Services.Commands
{
    public sealed class CreateTransferRumorCommand : IRequest<TransferRumor>
    {
        public required Guid PlayerId { get; init; }
        public required Guid SourceTeamId { get; init; }
        public required Guid TargetTeamId { get; init; }
        public required string Source { get; init; }
        public required RumorCredibility Credibility { get; init; }
        public required RumorStatus Status { get; init; }
    }
}
