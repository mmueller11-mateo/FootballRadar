namespace FootballRadar.Admin.WebApp.Models
{
    public class CountryViewModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Code { get; set; }
        public required string Flag { get; set; }
    }
}
