using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.Queries;
using FootballRadar.Business.Entities.TeamEntities;
using MediatR;

namespace FootballRadar.Admin.Business.Services.QueryHandlers
{
    sealed class GetTeamsQueryHandler : IRequestHandler<GetTeamsQuery, IEnumerable<Team>>
    {
        private readonly ITeamRepository teamRepository;

        public GetTeamsQueryHandler(ITeamRepository teamRepository)
        {
            this.teamRepository = teamRepository;
        }

        public async Task<IEnumerable<Team>> Handle(GetTeamsQuery request, CancellationToken cancellationToken)
        {
            return await teamRepository.GetAllAsync();
        }
    }
}