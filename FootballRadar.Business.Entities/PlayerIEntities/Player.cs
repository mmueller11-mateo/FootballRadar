namespace FootballRadar.Business.Entities.PlayerIEntities
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Weight { get; set; }
        public int? Height { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public Guid NationalityCountryId { get; set; }
        public string? Photo { get; set; }
    }
}