using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.TippSpiel.Abstractions;
using FootballRadar.TippSpiel.Business.Queries;
using MediatR;

namespace FootballRadar.TippSpiel.Business.QueryHandlers
{
    public class GetPlayedMatchesQueryHandler : IRequestHandler<GetPlayedMatchesQuery, IEnumerable<TippMatch>>
    {
        private readonly ITippMatchRepository tippMatchRepository;

        public GetPlayedMatchesQueryHandler(ITippMatchRepository tippMatchRepository)
        {
            this.tippMatchRepository = tippMatchRepository;
        }

        public async Task<IEnumerable<TippMatch>> Handle(GetPlayedMatchesQuery request, CancellationToken cancellationToken)
        {
            var all = await tippMatchRepository.GetAllAsync();
            return all.Where(m => m.HomeScore.HasValue && m.AwayScore.HasValue);
        }
    }
}
