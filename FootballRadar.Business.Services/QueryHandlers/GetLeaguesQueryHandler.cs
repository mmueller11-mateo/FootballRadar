using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.LeagueEntities;
using FootballRadar.Business.Services.Queries;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    sealed class GetLeaguesQueryHandler : IRequestHandler<GetLeaguesQuery, IReadOnlyCollection<PublicLeague>>
    {
        private readonly ILeagueRepository _leagueRepository;

        public GetLeaguesQueryHandler(ILeagueRepository leagueRepository)
        {
            this._leagueRepository = leagueRepository;
        }

        public async Task<IReadOnlyCollection<PublicLeague>> Handle(GetLeaguesQuery request, CancellationToken cancellationToken)
        {
            return await _leagueRepository.GetLeaguesAsync();
        }
    }
}
