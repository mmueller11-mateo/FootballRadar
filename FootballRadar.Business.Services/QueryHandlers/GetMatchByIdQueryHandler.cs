using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.LeagueEntities;
using FootballRadar.Business.Services.Queries;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    internal sealed class GetMatchByIdQueryHandler : IRequestHandler<GetMatchByIdQuery, Match?>
    {
        private readonly IMatchRepository matchRepository;

        public GetMatchByIdQueryHandler(IMatchRepository matchRepository)
        {
            this.matchRepository = matchRepository;
        }

        public async Task<Match?> Handle(GetMatchByIdQuery request, CancellationToken cancellationToken)
        {
            return await matchRepository.GetByIdAsync(request.MatchId, cancellationToken);
        }
    }
}