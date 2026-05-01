using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.LeagueEntities;
using FootballRadar.Business.Services.Queries;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    sealed class GetMatchesBySeasonQueryHandler : IRequestHandler<GetMatchesBySeasonQuery, IEnumerable<Match>>
    {
        private readonly IMatchRepository _matchRepository;

        public GetMatchesBySeasonQueryHandler(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<IEnumerable<Match>> Handle(GetMatchesBySeasonQuery request, CancellationToken cancellationToken)
        {
            return await _matchRepository.GetByLeagueAsync(request.ApiLeagueId, request.Season);
        }
    }
}