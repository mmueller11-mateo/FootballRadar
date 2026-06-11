namespace FootballRadar.Business.Entities.TippSpiel
{
    public class BonusQuestion
    {
        public Guid Id { get; set; }
        public required string QuestionText { get; set; }
        public DateTimeOffset Deadline { get; set; }
        public Guid? CorrectAnswerTeamId { get; set; }
        public int Points { get; set; }
        public bool IsResolved { get; set; }
        public int SortOrder { get; set; }
    }
}
