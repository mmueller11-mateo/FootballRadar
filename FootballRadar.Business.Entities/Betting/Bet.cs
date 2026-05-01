namespace FootballRadar.Business.Entities.Betting
{
    public class Bet
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PredictionMarketId { get; set; }
        public decimal Credits { get; set; }
        public DateTimeOffset PlacedAt { get; set; }
    }
}
