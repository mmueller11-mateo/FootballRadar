namespace FootballRadar.Business.Entities.TippSpiel
{
    public class Tip
    {
        public Guid Id { get; set; }
        public Guid TipperId { get; set; }
        public Guid MatchId { get; set; }
        public int PredictedHome { get; set; }
        public int PredictedAway { get; set; }
        public int? Points { get; set; }
    }
}
