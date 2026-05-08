using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.Commands.Create;
using FootballRadar.Business.Entities.TeamEntities;
using MediatR;

namespace FootballRadar.Admin.Business.Services.CommandHandlers.Create
{
    sealed class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, Team>
    {
        private readonly ITeamRepository teamRepository;

        public CreateTeamCommandHandler(ITeamRepository teamRepository)
        {
            this.teamRepository = teamRepository;
        }

        public async Task<Team> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            var trimmedName = request.Name.Trim();
            var existing = await teamRepository.GetByNameAsync(trimmedName);
            if (existing != null)
            {
                throw new InvalidOperationException($"A Team with the name '{request.Name}' already exists.");
            }

            var team = new Team
            {
                Id = Guid.NewGuid(),
                Name = trimmedName,
                CountryId = request.CountryId,
                ApiTeamId = request.ApiTeamId,
                Logo = request.Logo,
                Code = request.Code
            };

            await teamRepository.AddAsync(team);
            return team;
        }
    }
}