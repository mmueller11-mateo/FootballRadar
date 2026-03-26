namespace FootballRadar.Business.Entities.TransferEntities
{
    public class TransferWindow
    {
        public Guid Id { get; set; }
        public Guid LeagueId { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
    }
}