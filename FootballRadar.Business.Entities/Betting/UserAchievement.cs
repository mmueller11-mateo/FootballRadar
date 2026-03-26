namespace FootballRadar.Business.Entities.Betting
{
    public class UserAchievement
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid AchievementId { get; set; }
        public DateTimeOffset EarnedAt { get; set; }
    }
}
