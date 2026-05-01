using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.LeagueEntities;
using FootballRadar.Business.Services.Queries;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    internal sealed class GetUpcomingMatchesQueryHandler : IRequestHandler<GetUpcomingMatchesQuery, IEnumerable<Match>>
    {
        private readonly IMatchRepository _matchRepository;

        public GetUpcomingMatchesQueryHandler(IMatchRepository matchRepository)
        {
            this._matchRepository = matchRepository;
        }

        public async Task<IEnumerable<Match>> Handle(GetUpcomingMatchesQuery request, CancellationToken cancellationToken)
        {
            return await _matchRepository.GetUpcomingMatches(cancellationToken);
        }
    }
}
