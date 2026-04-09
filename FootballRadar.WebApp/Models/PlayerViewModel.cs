namespace FootballRadar.WebApp.Models
{
    public class PlayerViewModel
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string? Photo { get; set; }
        public Guid Nationality { get; set; }
    }
}
