using FootballRadar.Abstractions;
using FootballRadar.Business.Services.Queries;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    public class GetRanglisteQueryHandler : IRequestHandler<GetRanglisteQuery, IEnumerable<RanglisteEntry>>
    {
        private readonly IWmTipRepository wmTipRepository;
        private readonly IUserRepository userRepository;

        public GetRanglisteQueryHandler(IWmTipRepository wmTipRepository, IUserRepository userRepository)
        {
            this.wmTipRepository = wmTipRepository;
            this.userRepository = userRepository;
        }

        public async Task<IEnumerable<RanglisteEntry>> Handle(GetRanglisteQuery request, CancellationToken cancellationToken)
        {
            var tips = await wmTipRepository.GetAllAsync(cancellationToken);

            var grouped = tips
                .GroupBy(t => t.UserId)
                .Select(async g =>
                {
                    var user = await userRepository.GetByIdAsync(g.Key, cancellationToken);
                    return new RanglisteEntry
                    {
                        TipperName = user?.Name ?? "Unbekannt",
                        TotalPoints = g.Sum(t => t.Points ?? 0),
                        TipsCount = g.Count()
                    };
                });

            var entries = await Task.WhenAll(grouped);
            return entries.OrderByDescending(e => e.TotalPoints);
        }
    }
}