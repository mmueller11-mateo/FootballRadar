namespace FootballRadar.Business.Entities.ManagerEntities
{
    public class ManagerStatistics
    {
        public Guid Id { get; set; }
        public Guid ManagerId { get; set; }
        public Guid TeamId { get; set; }
        public Guid SeasonId { get; set; }
        public int Matches { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
    }
}
