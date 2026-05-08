using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.Queries;
using FootballRadar.Business.Entities;
using MediatR;

namespace FootballRadar.Admin.Business.Services.QueryHandlers
{
    sealed class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, IEnumerable<Country>>
    {
        private readonly ICountryRepository countryRepository;

        public GetCountriesQueryHandler(ICountryRepository countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        public async Task<IEnumerable<Country>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            return await countryRepository.GetAllAsync();
        }
    }
}
