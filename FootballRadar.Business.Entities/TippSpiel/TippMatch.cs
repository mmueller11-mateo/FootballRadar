namespace FootballRadar.Business.Entities.TippSpiel
{
    public class TippMatch
    {
        public Guid Id { get; set; }
        public string HomeTeam { get; set; } = string.Empty;
        public string AwayTeam { get; set; } = string.Empty;
        public DateTime KickoffUtc { get; set; }
        public string Phase { get; set; } = string.Empty;
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
    }
}