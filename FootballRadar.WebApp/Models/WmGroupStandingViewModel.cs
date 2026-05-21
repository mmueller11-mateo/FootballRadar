using FootballRadar.WebApp.Models.FootballRadar.WebApp.Models;

namespace FootballRadar.WebApp.Models
{
    public class WmGroupStandingViewModel
    {
        public string GroupName { get; set; } = "";
        public List<WmTeamStandingRow> Standings { get; set; } = [];
        public List<WmMatchViewModel> Matches { get; set; } = [];
    }
}
