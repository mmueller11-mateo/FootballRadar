namespace FootballRadar.Admin.WebApp.Models
{
    public class LeagueViewModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string CountryName { get; set; }
        public required string Logo { get; set; }
    }
}