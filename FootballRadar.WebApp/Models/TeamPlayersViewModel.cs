namespace FootballRadar.WebApp.Models
{
    public class TeamPlayersViewModel
    {
        public int ApiTeamId { get; set; }
        public IReadOnlyCollection<PlayerViewModel> Players { get; set; } = new List<PlayerViewModel>();
    }

}
