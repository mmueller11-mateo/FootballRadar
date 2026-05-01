namespace FootballRadar.WebApp.Models
{
    public class StandingsViewModel
    {
        public int LeagueId { get; set; }
        public int Season { get; set; }
        public IEnumerable<StandingViewModel> Standings { get; set; } = new List<StandingViewModel>();
        public IEnumerable<int> AvailableSeasons => Enumerable.Range(2022, 5).Reverse().ToList();
    }
}
