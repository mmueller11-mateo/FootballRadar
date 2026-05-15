using System.ComponentModel.DataAnnotations;

namespace FootballRadar.TippSpiel.WebApp.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name ist erforderlich.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Passwort ist erforderlich.")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Passwort muss mindestens 4 Zeichen lang sein.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bitte Passwort bestätigen.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwörter stimmen nicht überein.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
