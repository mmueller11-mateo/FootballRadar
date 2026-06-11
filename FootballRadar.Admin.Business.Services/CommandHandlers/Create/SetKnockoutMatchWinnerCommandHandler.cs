using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.Commands.Create;
using MediatR;

namespace FootballRadar.Admin.Business.Services.CommandHandlers.Create
{
    public class SetKnockoutMatchWinnerCommandHandler : IRequestHandler<SetKnockoutMatchWinnerCommand>
    {
        private readonly IMatchRepository matchRepository;
        private readonly IWmTipRepository wmTipRepository;

        public SetKnockoutMatchWinnerCommandHandler(
            IMatchRepository matchRepository,
            IWmTipRepository wmTipRepository)
        {
            this.matchRepository = matchRepository;
            this.wmTipRepository = wmTipRepository;
        }

        public async Task Handle(SetKnockoutMatchWinnerCommand request, CancellationToken cancellationToken)
        {
            var match = await matchRepository.GetByIdAsync(request.MatchId, cancellationToken);
            if (match == null) return;

            // Symbolisches Resultat: 1:0 Heim oder 0:1 Auswärts
            match.HomeGoals = request.Winner == "home" ? 1 : 0;
            match.AwayGoals = request.Winner == "away" ? 1 : 0;
            match.Status = "FT";
            await matchRepository.UpdateAsync(match, cancellationToken);

            // Punkte berechnen: richtiger Sieger = 3 Pkt
            var tips = await wmTipRepository.GetByMatchIdAsync(request.MatchId, cancellationToken);
            foreach (var tip in tips)
            {
                string tippedWinner = tip.HomeGoals > tip.AwayGoals ? "home" : "away";
                tip.Points = tippedWinner == request.Winner ? 3 : 0;
                await wmTipRepository.UpdateAsync(tip, cancellationToken);
            }
        }
    }
}
