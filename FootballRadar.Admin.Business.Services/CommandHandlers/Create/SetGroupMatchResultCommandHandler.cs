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

            match.HomeGoals = request.HomeGoals;
            match.AwayGoals = request.AwayGoals;
            match.Status = "FT";
            await matchRepository.UpdateAsync(match, cancellationToken);

            var tips = await wmTipRepository.GetByMatchIdAsync(request.MatchId, cancellationToken);
            foreach (var tip in tips)
            {
                var pts = CalculatePoints(tip.HomeGoals, tip.AwayGoals, request.HomeGoals, request.AwayGoals);
                tip.Points = pts;
                await wmTipRepository.UpdateAsync(tip, cancellationToken);
            }
        }

        private static int CalculatePoints(int predHome, int predAway, int actHome, int actAway)
        {
            int actualDiff = actHome - actAway;
            int predictedDiff = predHome - predAway;

            if (Math.Sign(predictedDiff) != Math.Sign(actualDiff)) return 0;
            if (predHome == actHome && predAway == actAway) return 4;
            if (actualDiff == 0) return 2;      // Unentschieden Tendenz → 2, keine Differenz-Stufe
            if (predictedDiff == actualDiff) return 3;  // Sieg Tordifferenz → 3
            return 2;                                    // Sieg Tendenz → 2
        }
    }
}