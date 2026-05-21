namespace FootballRadar.Business.Entities.TippSpiel
{
    public class WmTip
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid WmMatchId { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public int? Points { get; set; }
        public DateTimeOffset SubmittedAt { get; set; }
    }
}
