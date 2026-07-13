using FootballRadar.Abstractions;
using FootballRadar.Business.Services.Queries;
using FootballRadar.Business.Services.TippSpiel;
using FootballRadar.Data.Repositories;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    public class GetRanglisteQueryHandler : IRequestHandler<GetRanglisteQuery, IEnumerable<RanglisteEntry>>
    {
        private readonly IWmTipRepository wmTipRepository;
        private readonly IUserRepository userRepository;
        private readonly IMatchRepository matchRepository;
        private readonly IBonusTipRepository bonusTipRepository;

        public GetRanglisteQueryHandler(IWmTipRepository wmTipRepository, IUserRepository userRepository, IMatchRepository matchRepository, IBonusTipRepository bonusTipRepository)
        {
            this.wmTipRepository = wmTipRepository;
            this.userRepository = userRepository;
            this.matchRepository = matchRepository;
            this.bonusTipRepository = bonusTipRepository;
        }

        public async Task<IEnumerable<RanglisteEntry>> Handle(GetRanglisteQuery request, CancellationToken cancellationToken)
        {
            var tips = await wmTipRepository.GetAllAsync(cancellationToken);
            var matches = await matchRepository.GetAllAsync(cancellationToken);
            var users = await userRepository.GetAllAsync(cancellationToken);
            var bonusTips = await bonusTipRepository.GetAllAsync(cancellationToken);
            var matchCache = matches.ToDictionary(m => m.Id);
            var userCache = users.ToDictionary(u => u.Id);

            var bonusPointsByUser = bonusTips
                .GroupBy(b => b.UserId)
                .ToDictionary(g => g.Key, g => g.Sum(b => b.Points ?? 0));

            var result = tips
                .GroupBy(t => t.UserId)
                .Select(g =>
                {
                    userCache.TryGetValue(g.Key, out var user);

                    int matchPoints = 0;

                    foreach (var tip in g)
                    {
                        if (!matchCache.TryGetValue(tip.WmMatchId, out var match))
                        {
                            Console.WriteLine($"Match fehlt: {tip.WmMatchId}");
                            continue;
                        }

                        var calculated = tip.IsKoMatch
                            ? KoScoringService.Calculate(tip, match)
                            : PredictionScoringService.Calculate(tip, match);

                        if (calculated != tip.Points)
                        {
                            Console.WriteLine(
                                $"{user?.Name}: Match {tip.WmMatchId}, gespeichert={tip.Points}, berechnet={calculated}");
                        }

                        matchPoints += calculated;
                    }

                    int bonusPoints = bonusPointsByUser.TryGetValue(g.Key, out var bp) ? bp : 0;

                    return new RanglisteEntry
                    {
                        TipperName = user?.Name ?? "Unbekannt",
                        TotalPoints = matchPoints + bonusPoints,
                        TipsCount = g.Count()
                    };
                })
                .OrderByDescending(x => x.TotalPoints)
                .ToList();

            return result;
        }
    }
}