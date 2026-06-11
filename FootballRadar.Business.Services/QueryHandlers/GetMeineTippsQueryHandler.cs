using FootballRadar.Abstractions;
using FootballRadar.Business.Services.Queries;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    public sealed class GetMeineTippsQueryHandler : IRequestHandler<GetMeineTippsQuery, IEnumerable<MeinTippEntry>>
    {
        private readonly IWmTipRepository wmTipRepository;
        private readonly IMatchRepository matchRepository;
        private readonly INationalTeamRepository nationalTeamRepository;

        public GetMeineTippsQueryHandler(IWmTipRepository wmTipRepository, IMatchRepository matchRepository, INationalTeamRepository nationalTeamRepository)
        {
            this.wmTipRepository = wmTipRepository;
            this.matchRepository = matchRepository;
            this.nationalTeamRepository = nationalTeamRepository;
        }

        public async Task<IEnumerable<MeinTippEntry>> Handle(GetMeineTippsQuery request, CancellationToken cancellationToken)
        {
            var tips = await wmTipRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            var matches = await matchRepository.GetAllAsync(cancellationToken);
            var teams = await nationalTeamRepository.GetAllAsync(cancellationToken);

            return tips.Select(tip =>
            {
                var match = matches.FirstOrDefault(m => m.Id == tip.WmMatchId);
                var home = teams.FirstOrDefault(t => t.Id == match?.HomeNationalTeamId);
                var away = teams.FirstOrDefault(t => t.Id == match?.AwayNationalTeamId);

                string homeName = home?.Name ?? match?.HomeQualificationCode ?? "?";
                string awayName = away?.Name ?? match?.AwayQualificationCode ?? "?";

                bool isKo = tip.IsKoMatch;

                string? predictedWinnerName = null;
                if (isKo && tip.PredictedWinnerId.HasValue)
                {
                    var winner = teams.FirstOrDefault(t => t.Id == tip.PredictedWinnerId);
                    predictedWinnerName = winner?.Name;
                }

                string? actualWinnerName = null;
                if (isKo && match?.HomeGoals.HasValue == true && match?.AwayGoals.HasValue == true)
                {
                    if (match.HomeGoals > match.AwayGoals)
                        actualWinnerName = home?.Name ?? match.HomeQualificationCode;
                    else if (match.AwayGoals > match.HomeGoals)
                        actualWinnerName = away?.Name ?? match.AwayQualificationCode;
                }

                return new MeinTippEntry
                {
                    HomeTeam = homeName,
                    AwayTeam = awayName,
                    KickoffUtc = match?.Date ?? DateTimeOffset.MinValue,
                    WmGroup = match?.WmGroup,
                    PredictedHome = tip.HomeGoals,
                    PredictedAway = tip.AwayGoals,
                    ActualHome = match?.HomeGoals,
                    ActualAway = match?.AwayGoals,
                    Points = tip.Points,
                    IsKoMatch = isKo,
                    PredictedWinnerName = predictedWinnerName,
                    ActualWinnerName = actualWinnerName
                };
            }).OrderByDescending(t => t.KickoffUtc);
        }
    }
}