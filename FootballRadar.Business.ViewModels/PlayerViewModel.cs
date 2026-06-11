namespace FootballRadar.Business.ViewModels
{
    public class PlayerViewModel
    {
        public required string Name { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public required string Photo { get; set; }
        public Guid Nationality { get; set; }
    }
}
