using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.TippSpiel.Abstractions;
using FootballRadar.TippSpiel.Business.Queries;
using MediatR;

namespace FootballRadar.TippSpiel.Business.QueryHandlers
{
    public class GetTipsByTipperQueryHandler : IRequestHandler<GetTipsByTipperQuery, IEnumerable<Tip>>
    {
        private readonly ITipRepository tipRepository;

        public GetTipsByTipperQueryHandler(ITipRepository tipRepository)
        {
            this.tipRepository = tipRepository;
        }

        public async Task<IEnumerable<Tip>> Handle(GetTipsByTipperQuery request, CancellationToken cancellationToken)
        {
            return await tipRepository.GetByTipperIdAsync(request.TipperId);
        }
    }
}
