namespace FootballRadar.Business.Entities.PlayerIEntities
{
    public class Player
    {
        public Guid Id { get; set; }
        public int ApiPlayerId { get; set; }

        public required string Name { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public DateTimeOffset BirthDate { get; set; }
        public Guid NationalityCountryId { get; set; }
        public required string Photo { get; set; }
    }
}