namespace FootballRadar.Business.Entities.PlayerIEntities
{
    public class PlayerMarketValue
    {
        public Guid Id { get; set; }
        public string Source { get; set; }
        public Guid PlayerId { get; set; }
        public DateOnly Date { get; set; }
        public Money Value { get; set; }
    }
}
