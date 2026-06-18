using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.Commands.Create;
using MediatR;

namespace FootballRadar.Admin.Business.Services.CommandHandlers.Create
{
    public class RecalculateAllPointsCommandHandler : IRequestHandler<RecalculateAllPointsCommand>
    {
        private readonly IMatchRepository matchRepository;
        private readonly IWmTipRepository wmTipRepository;

        public RecalculateAllPointsCommandHandler(
            IMatchRepository matchRepository,
            IWmTipRepository wmTipRepository)
        {
            this.matchRepository = matchRepository;
            this.wmTipRepository = wmTipRepository;
        }

        public async Task Handle(RecalculateAllPointsCommand request, CancellationToken cancellationToken)
        {
            var matches = await matchRepository.GetByStatusAsync("FT", cancellationToken);

            foreach (var match in matches)
            {
                var tips = await wmTipRepository.GetByMatchIdAsync(match.Id, cancellationToken);
                foreach (var tip in tips)
                {
                    tip.Points = CalculatePoints(tip.HomeGoals, tip.AwayGoals, match.HomeGoals!.Value, match.AwayGoals!.Value);
                    await wmTipRepository.UpdateAsync(tip, cancellationToken);
                }
            }
        }

        private static int CalculatePoints(int predHome, int predAway, int actHome, int actAway)
        {
            int actualDiff = actHome - actAway;
            int predictedDiff = predHome - predAway;

            if (Math.Sign(predictedDiff) != Math.Sign(actualDiff)) return 0;
            if (predHome == actHome && predAway == actAway) return 4;
            if (actualDiff == 0) return 2;             // NEU: Unentschieden → 2, keine Differenz-Stufe
            if (predictedDiff == actualDiff) return 3;
            return 2;
        }
    }
}