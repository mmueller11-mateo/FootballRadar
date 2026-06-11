namespace FootballRadar.Business.Entities.TippSpiel
{
    public class BonusTip
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BonusQuestionId { get; set; }
        public Guid AnswerTeamId { get; set; }
        public int? Points { get; set; }
        public DateTimeOffset SubmittedAt { get; set; }
    }
}
