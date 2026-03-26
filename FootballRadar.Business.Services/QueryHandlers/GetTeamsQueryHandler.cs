using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.TeamEntities;
using FootballRadar.Business.Services.Queries;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    sealed class GetTeamsQueryHandler : IRequestHandler<GetTeamsQuery, IReadOnlyCollection<Team>>
    {
        private readonly ITeamRepository _teamRepository;

        public GetTeamsQueryHandler(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<IReadOnlyCollection<Team>> Handle(GetTeamsQuery request, CancellationToken cancellationToken)
        {
            return await _teamRepository.GetAllAsync();
        }
    }
}