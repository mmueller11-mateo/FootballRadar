namespace FootballRadar.Business.Entities.Betting
{
    public class AutomaticBiddingSetting
    {
        public Guid UserId { get; set; }
        public Guid BiddingRoundId { get; set; }
        public bool IsEnabled { get; set; }
        public Money UpperLimit { get; set; }
        public Money Amount { get; set; }
    }
}
