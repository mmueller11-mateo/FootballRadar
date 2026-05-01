namespace FootballRadar.Business.Entities.TeamEntities
{
    public class Team
    {
        public Guid Id { get; set; }
        public int? ApiTeamId { get; set; }
        public required string Name { get; set; }
        public string? Code { get; set; }
        public required string Logo { get; set; }
        public Guid? CountryId { get; set; }
    }
}
