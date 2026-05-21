namespace FootballRadar.WebApp.Models
{
    public class WmTipViewModel
    {
        public Guid MatchId { get; set; }
        public int PredictedHome { get; set; }
        public int PredictedAway { get; set; }
    }
}
