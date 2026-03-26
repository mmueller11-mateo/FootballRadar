namespace FootballRadar.WebApp.Models
{
    public class StandingsViewModel
    {
        public int LeagueId { get; set; }
        public int Season { get; set; }
        public IReadOnlyCollection<StandingViewModel> Standings { get; set; } = new List<StandingViewModel>();
        public IReadOnlyCollection<int> AvailableSeasons => Enumerable.Range(2022, 5).Reverse().ToList();
    }
}
