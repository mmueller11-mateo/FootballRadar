using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.TippSpiel.Abstractions;
using FootballRadar.TippSpiel.Business.Commands;
using MediatR;

namespace FootballRadar.TippSpiel.Business.CommandHandlers
{
    public class SaveTipCommandHandler : IRequestHandler<SaveTipCommand>
    {
        private readonly ITipRepository tipRepository;

        public SaveTipCommandHandler(ITipRepository tipRepository)
        {
            this.tipRepository = tipRepository;
        }

        public async Task Handle(SaveTipCommand request, CancellationToken cancellationToken)
        {
            var existing = await tipRepository.GetByTipperAndMatchAsync(request.TipperId, request.MatchId);
            if (existing != null)
            {
                existing.PredictedHome = request.PredictedHome;
                existing.PredictedAway = request.PredictedAway;
                await tipRepository.UpdateAsync(existing);
            }
            else
            {
                var tip = new Tip
                {
                    Id = Guid.NewGuid(),
                    TipperId = request.TipperId,
                    MatchId = request.MatchId,
                    PredictedHome = request.PredictedHome,
                    PredictedAway = request.PredictedAway
                };
                await tipRepository.AddAsync(tip);
            }
        }
    }
}