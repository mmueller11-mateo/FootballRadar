namespace FootballRadar.Business.Entities.Betting
{
    public class TransferPredictionMarket : PredictionMarket
    {
        public Guid TransferRumorId { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
