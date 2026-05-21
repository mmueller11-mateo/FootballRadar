using FootballRadar.Business.Services.Queries;

namespace FootballRadar.WebApp.Models
{
    public class MeineTippsViewModel
    {
        public List<MeinTippEntry> Entries { get; set; } = [];
        public int TotalPoints => Entries.Sum(e => e.Points ?? 0);
        public int TippedCount => Entries.Count;
        public int SettledCount => Entries.Count(e => e.Points.HasValue);
    }
}
