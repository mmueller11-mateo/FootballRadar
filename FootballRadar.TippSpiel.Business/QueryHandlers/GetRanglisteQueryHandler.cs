using FootballRadar.TippSpiel.Abstractions;
using FootballRadar.TippSpiel.Business.Queries;
using MediatR;

namespace FootballRadar.TippSpiel.Business.QueryHandlers
{
    public class GetRanglisteQueryHandler : IRequestHandler<GetRanglisteQuery, IEnumerable<RanglisteEntry>>
    {
        private readonly ITipperRepository tipperRepository;
        private readonly ITipRepository tipRepository;

        public GetRanglisteQueryHandler(ITipperRepository tipperRepository, ITipRepository tipRepository)
        {
            this.tipperRepository = tipperRepository;
            this.tipRepository = tipRepository;
        }

        public async Task<IEnumerable<RanglisteEntry>> Handle(GetRanglisteQuery request, CancellationToken cancellationToken)
        {
            var tippers = await tipperRepository.GetAllAsync();
            var result = new List<RanglisteEntry>();

            foreach (var tipper in tippers)
            {
                var tips = await tipRepository.GetByTipperIdAsync(tipper.Id);
                var scoredTips = tips.Where(t => t.Points.HasValue).ToList();

                result.Add(new RanglisteEntry
                {
                    TipperName = tipper.Name,
                    TotalPoints = scoredTips.Sum(t => t.Points!.Value),
                    TipsCount = scoredTips.Count
                });
            }

            return result.OrderByDescending(r => r.TotalPoints);
        }
    }
}
