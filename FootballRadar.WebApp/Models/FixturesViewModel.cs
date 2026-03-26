namespace FootballRadar.WebApp.Models
{
    public class FixturesViewModel
    {
        public int LeagueId { get; set; }
        public string LeagueName { get; set; }
        public string? LeagueLogo { get; set; }
        public IReadOnlyCollection<FixtureViewModel> UpcomingFixtures { get; set; } = new List<FixtureViewModel>();
        public IReadOnlyCollection<FixtureViewModel> PastFixtures { get; set; } = new List<FixtureViewModel>();
        public IReadOnlyCollection<int> Seasons { get; set; } = new List<int>();
        public int SelectedSeason { get; set; }
    }
}
