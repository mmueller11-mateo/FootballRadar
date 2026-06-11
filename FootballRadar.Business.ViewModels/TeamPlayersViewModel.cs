namespace FootballRadar.Business.ViewModels
{
    public class TeamPlayersViewModel
    {
        public int ApiTeamId { get; set; }

        public int Season { get; set; }

        public IEnumerable<PlayerViewModel> Players { get; set; } = new List<PlayerViewModel>();
        public IEnumerable<int> AvailableSeasons { get; set; } = new List<int>();
    }
}