using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FootballRadar.Admin.WebApp.Models
{
    public class CreateLeagueViewModel
    {
        public string Name { get; set; }
        public Guid CountryId { get; set; }
        public int ApiLeagueId { get; set; }

        [ValidateNever]
        public List<CountryViewModel> Countries { get; set; }
    }
}
