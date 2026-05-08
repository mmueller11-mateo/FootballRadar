namespace FootballRadar.TippSpiel.WebApp.Models
{
    public class MeineTippsViewModel
    {
        public string TipperName { get; set; } = string.Empty;
        public List<MeineTippViewModel> Tips { get; set; } = new();
    }
}
