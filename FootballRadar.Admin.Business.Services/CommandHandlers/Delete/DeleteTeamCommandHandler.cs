using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.Commands.Delete;
using MediatR;

namespace FootballRadar.Admin.Business.Services.CommandHandlers.Delete
{
    sealed class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand, bool>
    {
        private readonly ITeamRepository teamRepository;

        public DeleteTeamCommandHandler(ITeamRepository teamRepository)
        {
            this.teamRepository = teamRepository;
        }

        public async Task<bool> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
        {
            var team = await teamRepository.GetByIdAsync(request.Id);
            if (team == null)
            {
                return false;
            }

            teamRepository.Delete(team);
            return true;
        }
    }
}