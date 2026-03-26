namespace FootballRadar.Business.Entities.LeagueEntities
{
    public class Season
    {
        public Guid Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Guid LeagueId { get; set; }
    }
}
