using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.Queries;
using FootballRadar.Business.Entities.LeagueEntities;
using MediatR;

namespace FootballRadar.Admin.Business.Services.QueryHandlers
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
            return await _leagueRepository.GetAllAsync();
        }
    }
}
