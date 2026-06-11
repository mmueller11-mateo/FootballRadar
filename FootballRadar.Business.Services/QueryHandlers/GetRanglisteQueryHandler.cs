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

        public GetRanglisteQueryHandler(
            IWmTipRepository wmTipRepository,
            IUserRepository userRepository,
            IMatchRepository matchRepository,
            IBonusTipRepository bonusTipRepository)
        {
            this.wmTipRepository = wmTipRepository;
            this.userRepository = userRepository;
            this.matchRepository = matchRepository;
            this.bonusTipRepository = bonusTipRepository;
        }

        public async Task<IEnumerable<RanglisteEntry>> Handle(
            GetRanglisteQuery request,
            CancellationToken cancellationToken)
        {
            var tips = await wmTipRepository.GetAllAsync(cancellationToken);
            var matches = await matchRepository.GetAllAsync(cancellationToken);
            var users = await userRepository.GetAllAsync(cancellationToken);
            var bonusTips = await bonusTipRepository.GetAllAsync(cancellationToken);

            var matchCache = matches.ToDictionary(m => m.Id);
            var userCache = users.ToDictionary(u => u.Id);

            // Bonuspunkte pro User summieren (nur aufgelöste Fragen haben Points != null)
            var bonusPointsByUser = bonusTips
                .GroupBy(b => b.UserId)
                .ToDictionary(g => g.Key, g => g.Sum(b => b.Points ?? 0));

            var result = tips
                .GroupBy(t => t.UserId)
                .Select(g =>
                {
                    userCache.TryGetValue(g.Key, out var user);

                    int matchPoints = g
                        .Where(t => matchCache.ContainsKey(t.WmMatchId))
                        .Sum(t =>
                        {
                            var match = matchCache[t.WmMatchId];
                            return t.IsKoMatch
                                ? KoScoringService.Calculate(t, match)
                                : PredictionScoringService.Calculate(t, match);
                        });

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