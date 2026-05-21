using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.Business.Services.Commands;
using MediatR;

namespace FootballRadar.Business.Services.CommandHandlers
{
    public class SaveWmTipCommandHandler : IRequestHandler<SaveWmTipCommand>
    {
        private readonly IWmTipRepository wmTipRepository;

        public SaveWmTipCommandHandler(IWmTipRepository wmTipRepository)
        {
            this.wmTipRepository = wmTipRepository;
        }

        public async Task Handle(SaveWmTipCommand request, CancellationToken cancellationToken)
        {
            await wmTipRepository.UpsertAsync(new WmTip
            {
                UserId = request.UserId,
                WmMatchId = request.MatchId,
                HomeGoals = request.HomeGoals,
                AwayGoals = request.AwayGoals
            }, cancellationToken);
        }
    }
}