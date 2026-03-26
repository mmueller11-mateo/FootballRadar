namespace FootballRadar.Business.Entities.Betting
{
    public class UserStreak
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
        public DateTimeOffset LastActivityAt { get; set; }
    }
}
