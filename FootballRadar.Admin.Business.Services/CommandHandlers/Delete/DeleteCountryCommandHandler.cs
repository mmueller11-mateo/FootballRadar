using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.Commands.Delete;
using MediatR;

namespace FootballRadar.Admin.Business.Services.CommandHandlers.Delete
{
    sealed class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, bool>
    {
        private readonly ICountryRepository countryRepository;

        public DeleteCountryCommandHandler(ICountryRepository countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        public async Task<bool> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
        {
            var country = await countryRepository.GetByIdAsync(request.Id);

            if (country == null)
            {
                return false;
            }

            countryRepository.Delete(country);
            return true;
        }
    }
}
