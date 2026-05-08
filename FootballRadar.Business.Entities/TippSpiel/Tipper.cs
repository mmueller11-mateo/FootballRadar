namespace FootballRadar.Business.Entities.TippSpiel
{
    public class Tipper
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime FirstSeenAt { get; set; }
    }
}
