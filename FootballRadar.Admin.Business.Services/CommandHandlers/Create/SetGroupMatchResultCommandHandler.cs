using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.Commands.Create;
using MediatR;

namespace FootballRadar.Admin.Business.Services.CommandHandlers.Create
{
    public class SetGroupMatchResultCommandHandler : IRequestHandler<SetGroupMatchResultCommand>
    {
        private readonly IMatchRepository matchRepository;
        private readonly IWmTipRepository wmTipRepository;

        public SetGroupMatchResultCommandHandler(
            IMatchRepository matchRepository,
            IWmTipRepository wmTipRepository)
        {
            this.matchRepository = matchRepository;
            this.wmTipRepository = wmTipRepository;
        }

        public async Task Handle(SetGroupMatchResultCommand request, CancellationToken cancellationToken)
        {
            var match = await matchRepository.GetByIdAsync(request.MatchId, cancellationToken);
            if (match == null) return;

            // Resultat speichern
            match.HomeGoals = request.HomeGoals;
            match.AwayGoals = request.AwayGoals;
            match.Status = "FT";
            await matchRepository.UpdateAsync(match, cancellationToken);

            // Punkte berechnen
            var tips = await wmTipRepository.GetByMatchIdAsync(request.MatchId, cancellationToken);
            foreach (var tip in tips)
            {
                bool exact = tip.HomeGoals == request.HomeGoals
                          && tip.AwayGoals == request.AwayGoals;

                if (exact)
                {
                    tip.Points = 3;
                }
                else
                {
                    int actualTendency = Math.Sign(request.HomeGoals - request.AwayGoals);
                    int tippedTendency = Math.Sign(tip.HomeGoals - tip.AwayGoals);
                    tip.Points = actualTendency == tippedTendency ? 1 : 0;
                }

                await wmTipRepository.UpdateAsync(tip, cancellationToken);
            }
        }
    }
}