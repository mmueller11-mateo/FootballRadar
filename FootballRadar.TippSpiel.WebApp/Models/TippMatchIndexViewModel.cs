using FootballRadar.Business.Entities.TippSpiel;

namespace FootballRadar.TippSpiel.WebApp.Models
{
    public class TippMatchIndexViewModel
    {
        public List<TippMatch> Matches { get; set; } = new();
        public string? TipperName { get; set; }
        public List<Tip> ExistingTips { get; set; } = new();
    }
}
