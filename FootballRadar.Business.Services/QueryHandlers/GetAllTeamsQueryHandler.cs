using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.TeamEntities;
using FootballRadar.Business.Services.Queries;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    sealed class GetAllTeamsQueryHandler : IRequestHandler<GetAllTeamsQuery, IEnumerable<Team>>
    {
        private readonly ITeamRepository _teamRepository;

        public GetAllTeamsQueryHandler(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<IEnumerable<Team>> Handle(GetAllTeamsQuery request, CancellationToken cancellationToken)
        {
            return await _teamRepository.GetAllAsync(cancellationToken);
        }
    }
}