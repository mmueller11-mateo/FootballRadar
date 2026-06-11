using FootballRadar.Business.Entities.TeamEntities;

namespace FootballRadar.Business.Entities.TippSpiel
{
    public class KoMatchResult
    {
        public Guid FixtureId { get; set; }
        public int ApiFixtureId { get; set; }
        public DateTimeOffset KickoffUtc { get; set; }
        public string? HomeQualificationCode { get; set; }
        public string? AwayQualificationCode { get; set; }

        // Aufgelöste Teams (null = noch nicht bekannt)
        public NationalTeam? HomeTeam { get; set; }
        public NationalTeam? AwayTeam { get; set; }

        // Bestehender Tipp des Users
        public WmTip? ExistingTip { get; set; }
    }
}
