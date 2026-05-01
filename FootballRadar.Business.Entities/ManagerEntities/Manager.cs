namespace FootballRadar.Business.Entities.ManagerEntities
{
    public class Manager
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public Guid NationalityCountryId { get; set; }
    }
}
