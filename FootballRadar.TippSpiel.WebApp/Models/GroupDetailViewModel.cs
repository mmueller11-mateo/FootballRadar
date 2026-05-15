using FootballRadar.Business.Entities.TippSpiel;

namespace FootballRadar.TippSpiel.WebApp.Models
{
    public class GroupDetailViewModel
    {
        public string GroupName { get; set; } = string.Empty;
        public string? TipperName { get; set; }
        public List<TippMatch> PastMatches { get; set; } = new();
        public List<TippMatch> UpcomingMatches { get; set; } = new();
        public List<Tip> ExistingTips { get; set; } = new();
    }
}
