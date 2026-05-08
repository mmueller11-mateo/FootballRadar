namespace FootballRadar.TippSpiel.WebApp.Models
{
    public class ResultatMatchViewModel
    {
        public string HomeTeam { get; set; } = string.Empty;
        public string AwayTeam { get; set; } = string.Empty;
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public DateTime KickoffUtc { get; set; }
        public string Phase { get; set; } = string.Empty;
        public List<ResultatTipViewModel> Tips { get; set; } = new();
    }
}
