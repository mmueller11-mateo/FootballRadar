namespace FootballRadar.Business.Entities
{
    public class Country
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Code { get; set; }
        public required string Flag { get; set; }
    }
}