using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.TippSpiel.Abstractions;
using FootballRadar.TippSpiel.Business.Commands;
using MediatR;

namespace FootballRadar.TippSpiel.Business.CommandHandlers
{
    public class CreateTipperCommandHandler : IRequestHandler<CreateTipperCommand, Tipper>
    {
        private readonly ITipperRepository tipperRepository;

        public CreateTipperCommandHandler(ITipperRepository tipperRepository)
        {
            this.tipperRepository = tipperRepository;
        }

        public async Task<Tipper> Handle(CreateTipperCommand request, CancellationToken cancellationToken)
        {
            var tipper = new Tipper
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                FirstSeenAt = DateTime.UtcNow
            };
            await tipperRepository.AddAsync(tipper);
            return tipper;
        }
    }
}
