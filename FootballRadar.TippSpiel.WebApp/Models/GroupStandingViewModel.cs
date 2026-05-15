namespace FootballRadar.TippSpiel.WebApp.Models
{
    public class GroupStandingViewModel
    {
        public string GroupName { get; set; } = string.Empty;
        public List<TeamStandingRow> Standings { get; set; } = new();
    }

}
