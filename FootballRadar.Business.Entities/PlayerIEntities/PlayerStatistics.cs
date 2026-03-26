namespace FootballRadar.Business.Entities.PlayerIEntities
{
    public class PlayerStatistics
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid TeamId { get; set; }
        public Guid SeasonId { get; set; }
        public int NumberOfMatches { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
    }
}
