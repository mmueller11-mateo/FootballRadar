namespace FootballRadar.Business.Entities
{
    public class Country
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Code { get; set; }
        public string? Flag { get; set; }
    }
}