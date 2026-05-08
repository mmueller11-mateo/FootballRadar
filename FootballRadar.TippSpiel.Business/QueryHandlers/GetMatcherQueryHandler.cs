using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.TippSpiel.Abstractions;
using FootballRadar.TippSpiel.Business.Queries;
using MediatR;

namespace FootballRadar.TippSpiel.Business.QueryHandlers
{
    public class GetMatchesQueryHandler : IRequestHandler<GetMatchesQuery, IEnumerable<TippMatch>>
    {
        private readonly ITippMatchRepository tippMatchRepository;

        public GetMatchesQueryHandler(ITippMatchRepository tippMatchRepository)
        {
            this.tippMatchRepository = tippMatchRepository;
        }

        public async Task<IEnumerable<TippMatch>> Handle(GetMatchesQuery request, CancellationToken cancellationToken)
        {
            return await tippMatchRepository.GetAllAsync();
        }
    }

}
