namespace FootballRadar.Business.Entities.Betting
{
    public class NotificationPreference
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string EventType { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
    }
}
