using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.LeagueEntities;
using FootballRadar.Business.Services.Queries;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    internal sealed class GetUpcomingMatchesQueryHandler : IRequestHandler<GetUpcomingMatchesQuery, IReadOnlyCollection<Match>>
    {
        private readonly IMatchRepository _matchRepository;

        public GetUpcomingMatchesQueryHandler(IMatchRepository matchRepository)
        {
            this._matchRepository = matchRepository;
        }

        public async Task<IReadOnlyCollection<Match>> Handle(GetUpcomingMatchesQuery request, CancellationToken cancellationToken)
        {
            return await _matchRepository.GetUpcomingMatches();
        }
    }
}
