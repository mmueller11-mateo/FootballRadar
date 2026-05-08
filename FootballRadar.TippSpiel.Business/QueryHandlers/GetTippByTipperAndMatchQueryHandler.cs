using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.TippSpiel.Abstractions;
using FootballRadar.TippSpiel.Business.Queries;
using MediatR;

namespace FootballRadar.TippSpiel.Business.QueryHandlers
{
    public class GetTipByTipperAndMatchQueryHandler : IRequestHandler<GetTipByTipperAndMatchQuery, Tip?>
    {
        private readonly ITipRepository tipRepository;

        public GetTipByTipperAndMatchQueryHandler(ITipRepository tipRepository)
        {
            this.tipRepository = tipRepository;
        }

        public async Task<Tip?> Handle(GetTipByTipperAndMatchQuery request, CancellationToken cancellationToken)
        {
            return await tipRepository.GetByTipperAndMatchAsync(request.TipperId, request.MatchId);
        }
    }
}