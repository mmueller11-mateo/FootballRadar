namespace FootballRadar.Business.Entities.Betting
{

    public class PrivateLeagueMember
    {
        public Guid Id { get; set; }
        public Guid PrivateLeagueId { get; set; }
        public Guid UserId { get; set; }
        public DateTimeOffset JoinedAt { get; set; }
    }
}
