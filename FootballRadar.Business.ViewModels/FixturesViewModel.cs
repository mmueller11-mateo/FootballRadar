using System.ComponentModel.DataAnnotations;

namespace FootballRadar.Business.ViewModels
{
    public class FixturesViewModel
    {
        public int LeagueId { get; set; }
        [Required]
        public string LeagueName { get; set; } = string.Empty;
        [Required]
        public string LeagueLogo { get; set; } = string.Empty;
        public IEnumerable<FixtureViewModel> UpcomingFixtures { get; set; } = new List<FixtureViewModel>();
        public IEnumerable<FixtureViewModel> PastFixtures { get; set; } = new List<FixtureViewModel>();
        public IEnumerable<int> Seasons { get; set; } = new List<int>();
        public int SelectedSeason { get; set; }
    }
}
