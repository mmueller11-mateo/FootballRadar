using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.Queries;
using FootballRadar.Business.Entities.TeamEntities;
using MediatR;

namespace FootballRadar.Admin.Business.Services.QueryHandlers
{
    sealed class GetNationalTeamsQueryHandler : IRequestHandler<GetNationalTeamsQuery, IEnumerable<NationalTeam>>
    {
        private readonly INationalTeamRepository nationalTeamRepository;

        public GetNationalTeamsQueryHandler(INationalTeamRepository nationalTeamRepository)
        {
            this.nationalTeamRepository = nationalTeamRepository;
        }

        public async Task<IEnumerable<NationalTeam>> Handle(GetNationalTeamsQuery request, CancellationToken cancellationToken)
        {
            return await nationalTeamRepository.GetAllAsync(cancellationToken);
        }
    }
}