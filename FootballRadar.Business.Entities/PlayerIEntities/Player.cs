namespace FootballRadar.Business.Entities.PlayerIEntities
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Weight { get; set; }
        public int Height { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public Guid NationalityCountryId { get; set; }
    }
}
