using System.ComponentModel.DataAnnotations;

namespace FootballRadar.TippSpiel.WebApp.Models
{
    public class EnterNameViewModel
    {
        public Guid MatchId { get; set; }

        [Required(ErrorMessage = "Bitte gib deinen Namen ein")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
