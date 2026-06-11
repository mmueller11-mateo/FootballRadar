using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.Commands.Create;
using FootballRadar.Business.Entities.TeamEntities;
using MediatR;

namespace FootballRadar.Admin.Business.Services.CommandHandlers.Create
{
    sealed class CreateNationalTeamCommandHandler : IRequestHandler<CreateNationalTeamCommand, NationalTeam>
    {
        private readonly INationalTeamRepository nationalTeamRepository;

        public CreateNationalTeamCommandHandler(INationalTeamRepository nationalTeamRepository)
        {
            this.nationalTeamRepository = nationalTeamRepository;
        }

        public async Task<NationalTeam> Handle(CreateNationalTeamCommand request, CancellationToken cancellationToken)
        {
            var trimmedName = request.Name.Trim();
            var existing = await nationalTeamRepository.GetByNameAsync(trimmedName, cancellationToken);
            if (existing != null)
            {
                throw new InvalidOperationException($"A NationalTeam with the name '{request.Name}' already exists.");
            }

            var nationalTeam = new NationalTeam
            {
                Id = Guid.NewGuid(),
                Name = trimmedName,
                CountryId = request.CountryId,
                Level = request.Level,
                Logo = request.Logo
            };

            await nationalTeamRepository.AddAsync(nationalTeam, cancellationToken);
            return nationalTeam;
        }
    }
}