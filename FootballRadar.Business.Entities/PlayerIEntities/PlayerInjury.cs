namespace FootballRadar.Business.Entities.PlayerIEntities
{
    public class PlayerInjury
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public string Description { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? ExpectedReturn { get; set; }
    }
}
