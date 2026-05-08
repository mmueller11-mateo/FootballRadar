using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.Commands.Delete;
using MediatR;

namespace FootballRadar.Admin.Business.Services.CommandHandlers.Delete
{
    sealed class DeleteLeagueCommandHandler : IRequestHandler<DeleteLeagueCommand, bool>
    {
        private readonly ILeagueRepository leagueRepository;

        public DeleteLeagueCommandHandler(ILeagueRepository leagueRepository)
        {
            this.leagueRepository = leagueRepository;
        }

        public async Task<bool> Handle(DeleteLeagueCommand request, CancellationToken cancellationToken)
        {
            var league = await leagueRepository.GetByIdAsync(request.Id);

            if (league == null)
            {
                return false;
            }

            leagueRepository.Delete(league);
            return true;
        }
    }
}
