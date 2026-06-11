using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.Commands.Create;
using FootballRadar.Business.Entities;
using MediatR;

namespace FootballRadar.Admin.Business.Services.CommandHandlers.Create
{
    sealed class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, Country>
    {
        private readonly ICountryRepository countryRepository;

        public CreateCountryCommandHandler(ICountryRepository countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        public async Task<Country> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            var trimmedName = request.Name.Trim();
            var existing = await countryRepository.GetByNameAsync(trimmedName, cancellationToken);

            if (existing != null)
            {
                throw new InvalidOperationException($"A country with this name: '{request.Name}' already exists.");
            }

            var country = new Country
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Code = request.Code,
                Flag = request.Flag
            };

            await countryRepository.AddAsync(country, cancellationToken);
            return country;
        }
    }
}