namespace FootballRadar.Business.Entities.LeagueEntities
{
    public class Tournament
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }

}
