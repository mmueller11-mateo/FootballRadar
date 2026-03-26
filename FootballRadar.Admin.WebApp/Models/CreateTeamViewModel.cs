using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FootballRadar.Admin.WebApp.Models
{
    public class CreateTeamViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public Guid CountryId { get; set; }

        public string? Logo { get; set; }

        public int? ApiTeamId { get; set; }

        [ValidateNever]
        public List<CountryOption> Countries { get; set; } = new();
    }
}