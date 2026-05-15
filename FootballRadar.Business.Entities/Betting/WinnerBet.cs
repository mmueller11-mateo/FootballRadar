using FootballRadar.Business.Entities.Betting.Enums;

namespace FootballRadar.Business.Entities.Betting
{
    public class WinnerBet : Bet
    {
        public MatchPrediction Prediction { get; set; }
    }
}
