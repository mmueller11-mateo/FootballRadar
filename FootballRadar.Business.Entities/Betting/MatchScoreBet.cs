namespace FootballRadar.Business.Entities.Betting
{
    public class MatchScoreBet : Bet
    {
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
    }
}
