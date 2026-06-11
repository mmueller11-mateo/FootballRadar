using FootballRadar.Business.Entities.Enums;

namespace FootballRadar.Business.ViewModels
{
    public class ResultatMatchViewModel
    {
        public string HomeTeam { get; set; } = "";
        public string AwayTeam { get; set; } = "";
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public DateTimeOffset KickoffUtc { get; set; }
        public string? WmGroup { get; set; }
        public WmPhase WmPhase { get; set; }
        public List<ResultatTipViewModel> Tips { get; set; } = [];
    }
}
