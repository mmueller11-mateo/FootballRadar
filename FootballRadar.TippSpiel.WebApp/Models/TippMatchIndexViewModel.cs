namespace FootballRadar.TippSpiel.WebApp.Models
{
    public class TippMatchIndexViewModel
    {
        public string? TipperName { get; set; }
        public List<GroupStandingViewModel> Groups { get; set; } = new();
    }
}
