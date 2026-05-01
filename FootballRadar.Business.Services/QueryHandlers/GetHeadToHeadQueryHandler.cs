using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.LeagueEntities;
using FootballRadar.Business.Services.Queries;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    internal sealed class GetHeadToHeadQueryHandler : IRequestHandler<GetHeadToHeadQuery, IEnumerable<Match>>
    {
        private readonly IMatchRepository _matchRepository;

        public GetHeadToHeadQueryHandler(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<IEnumerable<Match>> Handle(GetHeadToHeadQuery request, CancellationToken cancellationToken)
        {
            return await _matchRepository.GetHeadToHeadAsync(request.HomeTeamId, request.AwayTeamId, request.Limit, cancellationToken);
        }
    }
}