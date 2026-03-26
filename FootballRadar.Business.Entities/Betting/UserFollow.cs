namespace FootballRadar.Business.Entities.Betting
{
    public class UserFollow
    {
        public Guid Id { get; set; }
        public Guid FollowerId { get; set; }
        public Guid FollowedId { get; set; }
        public bool IsActive { get; set; }
    }
}
