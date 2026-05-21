using FootballRadar.Business.Entities.Enums;

namespace FootballRadar.Business.Entities.TeamEntities
{
    public sealed class NationalTeam
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Guid CountryId { get; set; }
        public NationalTeamLevel Level { get; set; }
        public required string Logo { get; set; }
    }
}
