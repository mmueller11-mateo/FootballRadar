using System.ComponentModel.DataAnnotations;

namespace FootballRadar.WebApp.Models
{
    public class StandingTeamViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Logo { get; set; } = string.Empty;
        public int? ApiTeamId { get; set; }
    }
}
