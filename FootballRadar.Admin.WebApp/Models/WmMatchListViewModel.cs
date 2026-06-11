namespace FootballRadar.Admin.WebApp.Models
{
    public class WmMatchListViewModel
    {
        public List<WmMatchAdminViewModel> GroupMatches { get; set; } = new();
        public List<WmMatchAdminViewModel> KnockoutMatches { get; set; } = new();
    }
}
