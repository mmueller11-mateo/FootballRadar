namespace FootballRadar.WebApp.Models
{
    public class WmIndexViewModel
    {
        public string TipperName { get; set; } = "";
        public List<WmGroupStandingViewModel> Groups { get; set; } = [];
        public KoBracketViewModel? KoBracket { get; set; }
        public bool AllGroupTipped { get; set; }
    }
}
