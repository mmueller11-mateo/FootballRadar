using FootballRadar.Abstractions;
using FootballRadar.Business.Services.Queries;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    internal sealed class GetSeasonsByLeagueQueryHandler : IRequestHandler<GetSeasonsByLeagueQuery, IEnumerable<int>>
    {
        private readonly IMatchRepository _matchRepository;

        public GetSeasonsByLeagueQueryHandler(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<IEnumerable<int>> Handle(GetSeasonsByLeagueQuery request, CancellationToken cancellationToken)
        {
            return await _matchRepository.GetSeasonsByLeagueAsync(request.ApiLeagueId);
        }
    }
}