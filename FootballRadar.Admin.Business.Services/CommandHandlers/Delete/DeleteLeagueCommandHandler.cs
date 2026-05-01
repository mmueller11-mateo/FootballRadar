using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.Commands.Delete;
using MediatR;

namespace FootballRadar.Admin.Business.Services.CommandHandlers.Delete
{
    sealed class DeleteLeagueCommandHandler : IRequestHandler<DeleteLeagueCommand, bool>
    {
        private readonly ILeagueRepository _leagueRepository;

        public DeleteLeagueCommandHandler(ILeagueRepository leagueRepository)
        {
            this._leagueRepository = leagueRepository;
        }

        public async Task<bool> Handle(DeleteLeagueCommand request, CancellationToken cancellationToken)
        {
            var league = await _leagueRepository.GetByIdAsync(request.Id);

            if (league == null)
            {
                return false;
            }

            _leagueRepository.Delete(league);
            return true;
        }
    }
}
