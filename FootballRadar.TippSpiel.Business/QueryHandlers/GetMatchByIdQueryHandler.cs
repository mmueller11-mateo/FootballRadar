using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.TippSpiel.Abstractions;
using FootballRadar.TippSpiel.Business.Queries;
using MediatR;

namespace FootballRadar.TippSpiel.Business.QueryHandlers
{
    public class GetMatchByIdQueryHandler : IRequestHandler<GetMatchByIdQuery, TippMatch?>
    {
        private readonly ITippMatchRepository tippMatchRepository;

        public GetMatchByIdQueryHandler(ITippMatchRepository tippMatchRepository)
        {
            this.tippMatchRepository = tippMatchRepository;
        }

        public async Task<TippMatch?> Handle(GetMatchByIdQuery request, CancellationToken cancellationToken)
        {
            return await tippMatchRepository.GetByIdAsync(request.MatchId);
        }
    }
}
