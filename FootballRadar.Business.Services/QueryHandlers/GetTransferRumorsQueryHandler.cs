using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.TransferEntities;
using FootballRadar.Business.Services.Queries;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    internal sealed class GetTransferRumorsQueryHandler : IRequestHandler<GetTransferRumorsQuery, IReadOnlyCollection<TransferRumor>>
    {
        private readonly ITransferRepository transferRepository;

        public GetTransferRumorsQueryHandler(ITransferRepository transferRepository)
        {
            this.transferRepository = transferRepository;
        }


        public async Task<IReadOnlyCollection<TransferRumor>> Handle(GetTransferRumorsQuery request, CancellationToken cancellationToken)
        {
            return await transferRepository.GetTransferRumors(request.RumorStatus);
        }
    }
}
