using FootballRadar.Business.Entities;
using FootballRadar.Business.Entities.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FootballRadar.Admin.WebApp.Models
{
    public class CreateNationalTeamViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public Guid CountryId { get; set; }
        public NationalTeamLevel Level { get; set; }
        [ValidateNever]
        public List<Country> Countries { get; set; } = new();

        public string? LogoUrl { get; set; }
        [ValidateNever]
        public List<NationalTeamLevel> Levels { get; set; } = Enum.GetValues<NationalTeamLevel>().ToList();
    }
}
