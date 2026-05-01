using FootballRadar.Abstractions;
using FootballRadar.Abstractions.Events;
using FootballRadar.Business.Entities.TransferEntities;
using FootballRadar.Business.Services.Commands;
using MediatR;
using My.Framework.EventHandling;

namespace FootballRadar.Business.Services.CommandHandlers
{
    internal sealed class CreateTransferRumorCommandHandler : IRequestHandler<CreateTransferRumorCommand, TransferRumor>
    {
        private readonly IEventPublisher eventPublisher;
        private readonly ITransferRepository transferRepository;

        public CreateTransferRumorCommandHandler(IEventPublisher eventPublisher, ITransferRepository transferRepository)
        {
            this.eventPublisher = eventPublisher;
            this.transferRepository = transferRepository;
        }

        public async Task<TransferRumor> Handle(CreateTransferRumorCommand request, CancellationToken cancellationToken)
        {
            var transferRumor = new TransferRumor
            {
                Id = Guid.NewGuid(),
                PlayerId = request.PlayerId,
                SourceTeamId = request.SourceTeamId,
                TargetTeamId = request.TargetTeamId,
                Credibility = request.Credibility,
                Source = request.Source,
                CreatedAt = DateTimeOffset.UtcNow,
                Status = request.Status
            };

            await transferRepository.AddTransferRumor(transferRumor, cancellationToken);


            await eventPublisher.Publish(new TransferRumorReported
            {
                TransferRumorId = transferRumor.Id
            }, cancellationToken);

            return transferRumor;
        }
    }
}
