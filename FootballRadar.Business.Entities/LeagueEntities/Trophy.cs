namespace FootballRadar.Business.Entities.LeagueEntities
{
    public class Trophy
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Guid SeasonId { get; set; }
        public Guid WinnerId { get; set; }
        public TrophyWinner WinnerType { get; set; }
    }
}
