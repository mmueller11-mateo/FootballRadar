namespace FootballRadar.TippSpiel.WebApp.Models
{
    public class MeineTippViewModel
    {
        public string HomeTeam { get; set; } = string.Empty;
        public string AwayTeam { get; set; } = string.Empty;
        public DateTime KickoffUtc { get; set; }
        public string Phase { get; set; } = string.Empty;
        public int PredictedHome { get; set; }
        public int PredictedAway { get; set; }
        public int? Points { get; set; }
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
    }

}
