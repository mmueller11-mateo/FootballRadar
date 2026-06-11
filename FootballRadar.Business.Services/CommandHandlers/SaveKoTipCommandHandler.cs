using FootballRadar.Abstractions;
using FootballRadar.Business.Services.Commands;
using MediatR;

namespace FootballRadar.Business.Services.CommandHandlers
{
    public class SaveKoTipCommandHandler : IRequestHandler<SaveKoTipCommand>
    {
        private readonly IWmTipRepository wmTipRepository;

        public SaveKoTipCommandHandler(IWmTipRepository wmTipRepository)
        {
            this.wmTipRepository = wmTipRepository;
        }

        public async Task Handle(SaveKoTipCommand request, CancellationToken cancellationToken)
        {
            var existing = await wmTipRepository.GetByUserAndMatchAsync(
                request.UserId, request.FixtureId, cancellationToken);

            if (existing != null)
            {
                existing.PredictedWinnerId = request.WinnerId;
                existing.SubmittedAt = DateTimeOffset.UtcNow;
                await wmTipRepository.UpdateAsync(existing, cancellationToken);
            }
            else
            {
                var tip = new WmTip
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    WmMatchId = request.FixtureId,
                    IsKoMatch = true,
                    PredictedWinnerId = request.WinnerId,
                    HomeGoals = 0,
                    AwayGoals = 0,
                    SubmittedAt = DateTimeOffset.UtcNow,
                };
                await wmTipRepository.AddAsync(tip, cancellationToken);
            }
        }
    }
}
