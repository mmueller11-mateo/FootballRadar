using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public class GetRanglisteQuery : IRequest<IEnumerable<RanglisteEntry>>
    {
    }

    public class RanglisteEntry
    {
        public string TipperName { get; set; } = string.Empty;
        public int TotalPoints { get; set; }
        public int TipsCount { get; set; }
    }
}
