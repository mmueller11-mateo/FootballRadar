using FootballRadar.Business.Entities.Betting.Enums;

namespace FootballRadar.Business.Entities.Betting
{
    public class TransferBet : Bet
    {
        public TransferPrediction Prediction { get; set; }
    }
}
