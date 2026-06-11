namespace FootballRadar.Business.Entities.TippSpiel
{
    public class KoBracketResult
    {
        public List<KoMatchResult> RoundOf32 { get; set; } = new();
        public List<KoMatchResult> RoundOf16 { get; set; } = new();
        public List<KoMatchResult> QuarterFinals { get; set; } = new();
        public List<KoMatchResult> SemiFinals { get; set; } = new();
        public KoMatchResult? ThirdPlace { get; set; }
        public KoMatchResult? Final { get; set; }
    }
}
