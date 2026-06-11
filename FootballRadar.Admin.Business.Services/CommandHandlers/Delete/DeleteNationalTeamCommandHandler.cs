using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.Commands.Delete;
using MediatR;

namespace FootballRadar.Admin.Business.Services.CommandHandlers.Delete
{
    sealed class DeleteNationalTeamCommandHandler : IRequestHandler<DeleteNationalTeamCommand, bool>
    {
        private readonly INationalTeamRepository nationalTeamRepository;

        public DeleteNationalTeamCommandHandler(INationalTeamRepository nationalTeamRepository)
        {
            this.nationalTeamRepository = nationalTeamRepository;
        }

        public async Task<bool> Handle(DeleteNationalTeamCommand request, CancellationToken cancellationToken)
        {
            var nationalTeam = await nationalTeamRepository.GetByIdAsync(request.Id, cancellationToken);

            if (nationalTeam == null)
            {
                return false;
            }

            nationalTeamRepository.Delete(nationalTeam, cancellationToken);
            return true;
        }
    }
}