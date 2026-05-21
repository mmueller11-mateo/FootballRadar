using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.LeagueEntities;
using FootballRadar.Business.Services.Queries;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    internal sealed class GetPlayedMatchesQueryHandler : IRequestHandler<GetPlayedMatchesQuery, IEnumerable<Match>>
    {
        private readonly IMatchRepository matchRepository;

        public GetPlayedMatchesQueryHandler(IMatchRepository matchRepository)
        {
            this.matchRepository = matchRepository;
        }

        public async Task<IEnumerable<Match>> Handle(GetPlayedMatchesQuery request, CancellationToken cancellationToken)
        {
            var matches = await matchRepository.GetAllAsync(cancellationToken);
            return matches.Where(m => m.WmPhase.HasValue && m.HomeGoals.HasValue && m.AwayGoals.HasValue);
        }
    }
}
