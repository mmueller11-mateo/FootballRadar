using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FootballRadar.Admin.WebApp.Models
{
    public class CreateLeagueViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public Guid CountryId { get; set; }
        public int ApiLeagueId { get; set; }
        [Required]
        public string Logo { get; set; } = string.Empty;

        [ValidateNever]
        public List<CountryViewModel> Countries { get; set; } = new();
    }
}
