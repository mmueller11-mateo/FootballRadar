using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.TippSpiel.Abstractions;
using FootballRadar.TippSpiel.Business.Queries;
using MediatR;

namespace FootballRadar.TippSpiel.Business.QueryHandlers
{
    public class GetTipperByNameQueryHandler : IRequestHandler<GetTipperByNameQuery, Tipper?>
    {
        private readonly ITipperRepository tipperRepository;

        public GetTipperByNameQueryHandler(ITipperRepository tipperRepository)
        {
            this.tipperRepository = tipperRepository;
        }

        public async Task<Tipper?> Handle(GetTipperByNameQuery request, CancellationToken cancellationToken)
        {
            return await tipperRepository.GetByNameAsync(request.Name);
        }
    }
}
