namespace FootballRadar.TippSpiel.WebApp.Models
{
    public class ResultatTipViewModel
    {
        public string TipperName { get; set; } = string.Empty;
        public int PredictedHome { get; set; }
        public int PredictedAway { get; set; }
        public int Points { get; set; }
    }
}
