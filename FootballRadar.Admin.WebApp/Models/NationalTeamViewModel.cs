namespace FootballRadar.Admin.WebApp.Models
{
    public class NationalTeamViewModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Country { get; set; }
        public required string Level { get; set; }
        public required string Logo { get; set; }
    }
}
