using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.LeagueEntities;
using FootballRadar.Business.Services.Queries;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    internal sealed class GetMatchesQueryHandler : IRequestHandler<GetMatchesQuery, IEnumerable<Match>>
    {
        private readonly IMatchRepository matchRepository;

        public GetMatchesQueryHandler(IMatchRepository matchRepository)
        {
            this.matchRepository = matchRepository;
        }

        public async Task<IEnumerable<Match>> Handle(GetMatchesQuery request, CancellationToken cancellationToken)
        {
            return await matchRepository.GetAllAsync();
        }
    }

}
