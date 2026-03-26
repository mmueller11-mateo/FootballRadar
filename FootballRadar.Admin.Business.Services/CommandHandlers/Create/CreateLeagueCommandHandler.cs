using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.Commands.Create;
using FootballRadar.Business.Entities.LeagueEntities;
using MediatR;

namespace FootballRadar.Admin.Business.Services.CommandHandlers.Create
{
    sealed class CreateLeagueCommandHandler : IRequestHandler<CreateLeagueCommand, PublicLeague>
    {
        private readonly ILeagueRepository _leagueRepository;

        public CreateLeagueCommandHandler(ILeagueRepository leagueRepository)
        {
            this._leagueRepository = leagueRepository;
        }

        public async Task<PublicLeague> Handle(CreateLeagueCommand request, CancellationToken cancellationToken)
        {
            var trimmedName = request.Name.Trim();
            var existing = await _leagueRepository.GetByNameAsync(trimmedName);

            if (existing != null)
            {
                throw new InvalidOperationException($"A league with this name: '{request.Name}' already exists.");
            }

            var league = new PublicLeague
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                CountryId = request.CountryId,
                ApiLeagueId = request.ApiLeagueId

            };


            await _leagueRepository.AddAsync(league);
            return league;
        }
    }
}