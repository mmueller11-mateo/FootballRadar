using System.ComponentModel.DataAnnotations;

namespace FootballRadar.TippSpiel.WebApp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Name ist erforderlich.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Passwort ist erforderlich.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
