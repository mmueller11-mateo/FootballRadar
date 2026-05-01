using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.Commands.Delete;
using MediatR;

namespace FootballRadar.Admin.Business.Services.CommandHandlers.Delete
{
    sealed class DeleteNationalTeamCommandHandler : IRequestHandler<DeleteNationalTeamCommand, bool>
    {
        private readonly INationalTeamRepository _nationalTeamRepository;

        public DeleteNationalTeamCommandHandler(INationalTeamRepository nationalTeamRepository)
        {
            _nationalTeamRepository = nationalTeamRepository;
        }

        public async Task<bool> Handle(DeleteNationalTeamCommand request, CancellationToken cancellationToken)
        {
            var nationalTeam = await _nationalTeamRepository.GetByIdAsync(request.Id);

            if (nationalTeam == null)
            {
                return false;
            }

            _nationalTeamRepository.Delete(nationalTeam);
            return true;
        }
    }
}