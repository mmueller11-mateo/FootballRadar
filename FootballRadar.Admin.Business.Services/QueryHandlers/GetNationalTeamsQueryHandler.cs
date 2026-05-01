using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.Queries;
using FootballRadar.Business.Entities.TeamEntities;
using MediatR;

namespace FootballRadar.Admin.Business.Services.QueryHandlers
{
    sealed class GetNationalTeamsQueryHandler : IRequestHandler<GetNationalTeamsQuery, IEnumerable<NationalTeam>>
    {
        private readonly INationalTeamRepository _nationalTeamRepository;

        public GetNationalTeamsQueryHandler(INationalTeamRepository nationalTeamRepository)
        {
            _nationalTeamRepository = nationalTeamRepository;
        }

        public async Task<IEnumerable<NationalTeam>> Handle(GetNationalTeamsQuery request, CancellationToken cancellationToken)
        {
            return await _nationalTeamRepository.GetAllAsync();
        }
    }
}