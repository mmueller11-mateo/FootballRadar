namespace FootballRadar.Business.Entities.TeamEntities
{
    public class Team
    {
        public Guid Id { get; set; }
        public int? ApiTeamId { get; set; }
        public string Name { get; set; }
        public string? Code { get; set; }
        public string? Logo { get; set; }
        public Guid? CountryId { get; set; }
    }
}
