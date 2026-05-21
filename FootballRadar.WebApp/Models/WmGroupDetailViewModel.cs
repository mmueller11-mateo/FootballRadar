using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.WebApp.Models.FootballRadar.WebApp.Models;

namespace FootballRadar.WebApp.Models
{
    public class WmGroupDetailViewModel
    {
        public string GroupName { get; set; } = "";
        public string TipperName { get; set; } = "";
        public List<WmMatchViewModel> PastMatches { get; set; } = [];
        public List<WmMatchViewModel> UpcomingMatches { get; set; } = [];
        public List<WmTip> ExistingTips { get; set; } = [];
        public List<WmTeamStandingRow> GroupStandings { get; set; } = new();
    }
}
