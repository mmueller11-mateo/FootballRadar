using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.Commands.Create;
using FootballRadar.Business.Entities;
using MediatR;

sealed class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, Country>
{
    private readonly ICountryRepository _countryRepository;

    public CreateCountryCommandHandler(ICountryRepository countryRepository)
    {
        this._countryRepository = countryRepository;
    }

    public async Task<Country> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
    {
        var trimmedName = request.Name.Trim();
        var existing = await _countryRepository.GetByNameAsync(trimmedName);

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

        await _countryRepository.AddAsync(country);
        return country;
    }
}
