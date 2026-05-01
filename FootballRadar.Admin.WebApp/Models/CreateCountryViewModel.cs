using System.ComponentModel.DataAnnotations;

namespace FootballRadar.Admin.WebApp.Models
{
    public class CreateCountryViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Code { get; set; } = string.Empty;

        [Required]
        public string Flag { get; set; } = string.Empty;
    }
}
