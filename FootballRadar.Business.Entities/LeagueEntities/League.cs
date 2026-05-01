namespace FootballRadar.Business.Entities.LeagueEntities
{
    public abstract class League
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Guid CountryId { get; set; }
        public required string Logo { get; set; }
    }
}
