using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.Commands.Create;
using FootballRadar.Business.Services.TippSpiel;
using MediatR;

namespace FootballRadar.Admin.Business.Services.CommandHandlers.Create
{
    public class SetKnockoutMatchResultCommandHandler : IRequestHandler<SetKnockoutMatchResultCommand>
    {
        private readonly IMatchRepository matchRepository;
        private readonly IWmTipRepository wmTipRepository;

        public SetKnockoutMatchResultCommandHandler(
            IMatchRepository matchRepository,
            IWmTipRepository wmTipRepository)
        {
            this.matchRepository = matchRepository;
            this.wmTipRepository = wmTipRepository;
        }

        public async Task Handle(SetKnockoutMatchResultCommand request, CancellationToken cancellationToken)
        {
            var match = await matchRepository.GetByIdAsync(request.MatchId, cancellationToken);
            if (match == null) return;

            match.HomeGoals = request.HomeGoals;
            match.AwayGoals = request.AwayGoals;
            match.Status = "FT";
            await matchRepository.UpdateAsync(match, cancellationToken);

            var tips = await wmTipRepository.GetByMatchIdAsync(request.MatchId, cancellationToken);
            foreach (var tip in tips)
            {
                tip.Points = KoScoringService.Calculate(tip, match);
                await wmTipRepository.UpdateAsync(tip, cancellationToken);
            }
        }
    }
}
