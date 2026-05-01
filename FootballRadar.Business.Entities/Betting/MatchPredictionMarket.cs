namespace FootballRadar.Business.Entities.Betting
{
    public class MatchPredictionMarket : PredictionMarket
    {
        public Guid MatchId { get; set; }
        public bool IsSettled { get; set; } = false;

    }
}
