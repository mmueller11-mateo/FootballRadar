namespace FootballRadar.WebApp.Models
{
    public class KoBracketViewModel
    {
        public List<KoMatchViewModel> RoundOf32 { get; set; } = new();
        public List<KoMatchViewModel> RoundOf16 { get; set; } = new();
        public List<KoMatchViewModel> QuarterFinals { get; set; } = new();
        public List<KoMatchViewModel> SemiFinals { get; set; } = new();
        public KoMatchViewModel? ThirdPlace { get; set; }
        public KoMatchViewModel? Final { get; set; }
    }
}
