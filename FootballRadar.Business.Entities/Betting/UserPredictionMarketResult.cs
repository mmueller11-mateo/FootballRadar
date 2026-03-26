using FootballRadar.Business.Entities.Betting.Enums;

namespace FootballRadar.Business.Entities.Betting
{
    public class UserPredictionMarketResult
    {
        public Guid Id { get; set; }
        public Guid UserId  { get; set; }
        public Guid PredictionMarketId { get; set; }
        public PredictionMarketOutcome Status { get; set; }
        public Money Reward {  get; set; }
    }
}
