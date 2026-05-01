namespace FootballRadar.Admin.WebApp.Models
{
    public class TeamViewModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string CountryFlag { get; set; }
        public required string Logo { get; set; }
        public required string Code { get; set; }
    }
}
