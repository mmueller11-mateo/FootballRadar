namespace FootballRadar.Business.Services.Queries
{
    public class MeinTippEntry
    {
        public string HomeTeam { get; set; } = "";
        public string AwayTeam { get; set; } = "";
        public DateTimeOffset KickoffUtc { get; set; }
        public string? WmGroup { get; set; }
        public int PredictedHome { get; set; }
        public int PredictedAway { get; set; }
        public int? ActualHome { get; set; }
        public int? ActualAway { get; set; }
        public int? Points { get; set; }
    }
}
