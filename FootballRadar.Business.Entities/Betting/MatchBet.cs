using FootballRadar.Business.Entities.Betting.Enums;

namespace FootballRadar.Business.Entities.Betting
{
    public class MatchBet : Bet
    {
        public MatchPrediction Prediction { get; set; }
    }
}
