namespace FootballRadar.Business.Entities.Betting
{
    public class LeaderboardEntry
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int Season { get; set; }
        public int Rank { get; set; }
        public int TotalWins { get; set; }
        public decimal TotalEarned { get; set; }
    }
}
