using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.Business.Services.Queries;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    internal sealed class GetTipsByMatchQueryHandler : IRequestHandler<GetTipsByMatchQuery, IEnumerable<WmTip>>
    {
        private readonly IWmTipRepository wmTipRepository;

        public GetTipsByMatchQueryHandler(IWmTipRepository wmTipRepository)
        {
            this.wmTipRepository = wmTipRepository;
        }

        public async Task<IEnumerable<WmTip>> Handle(GetTipsByMatchQuery request, CancellationToken cancellationToken)
        {
            return await wmTipRepository.GetByMatchIdAsync(request.MatchId, cancellationToken);
        }
    }
}
