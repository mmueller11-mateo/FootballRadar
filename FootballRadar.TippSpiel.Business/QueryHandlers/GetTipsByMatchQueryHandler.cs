using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.TippSpiel.Abstractions;
using FootballRadar.TippSpiel.Business.Queries;
using MediatR;

namespace FootballRadar.TippSpiel.Business.QueryHandlers
{
    public class GetTipsByMatchQueryHandler : IRequestHandler<GetTipsByMatchQuery, IEnumerable<Tip>>
    {
        private readonly ITipRepository tipRepository;

        public GetTipsByMatchQueryHandler(ITipRepository tipRepository)
        {
            this.tipRepository = tipRepository;
        }

        public async Task<IEnumerable<Tip>> Handle(GetTipsByMatchQuery request, CancellationToken cancellationToken)
        {
            return await tipRepository.GetByMatchIdAsync(request.MatchId);
        }
    }
}
