namespace FootballRadar.Business.Entities.LeagueEntities
{
    public abstract class League
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? CountryId { get; set; }
        public string Logo { get; set; }
    }
}
