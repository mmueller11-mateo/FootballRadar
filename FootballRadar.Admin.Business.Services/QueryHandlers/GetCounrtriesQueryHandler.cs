using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.Queries;
using FootballRadar.Business.Entities;
using MediatR;

namespace FootballRadar.Admin.Business.Services.QueryHandlers
{
    sealed class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, IReadOnlyCollection<Country>>
    {
        private readonly ICountryRepository _repository;

        public GetCountriesQueryHandler(ICountryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyCollection<Country>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
