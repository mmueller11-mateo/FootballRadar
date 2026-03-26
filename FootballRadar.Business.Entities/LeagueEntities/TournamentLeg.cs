namespace FootballRadar.Business.Entities.LeagueEntities
{
    public class TournamentLeg
    {
        public Guid Id { get; set; }
        public Guid TournamentId { get; set; }
        public Guid HostCountryId { get; set; }
    }
}
