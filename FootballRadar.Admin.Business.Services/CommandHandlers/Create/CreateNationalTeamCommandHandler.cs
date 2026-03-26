using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.Commands.Create;
using FootballRadar.Business.Entities.TeamEntities;
using MediatR;

namespace FootballRadar.Admin.Business.Services.CommandHandlers.Create
{
    sealed class CreateNationalTeamCommandHandler : IRequestHandler<CreateNationalTeamCommand, NationalTeam>
    {
        private readonly INationalTeamRepository _nationalTeamRepository;

        public CreateNationalTeamCommandHandler(INationalTeamRepository nationalTeamRepository)
        {
            _nationalTeamRepository = nationalTeamRepository;
        }

        public async Task<NationalTeam> Handle(CreateNationalTeamCommand request, CancellationToken cancellationToken)
        {
            var trimmedName = request.Name.Trim();
            var existing = await _nationalTeamRepository.GetByNameAsync(trimmedName);
            if (existing != null)
            {
                throw new InvalidOperationException($"A NationalTeam with the name '{request.Name}' already exists.");
            }

            var nationalTeam = new NationalTeam
            {
                Id = Guid.NewGuid(),
                Name = trimmedName,
                CountryId = request.CountryId,
                Level = request.Level
            };

            await _nationalTeamRepository.AddAsync(nationalTeam);
            return nationalTeam;
        }
    }
}