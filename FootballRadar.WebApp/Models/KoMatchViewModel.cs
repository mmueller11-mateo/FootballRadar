namespace FootballRadar.WebApp.Models
{
    public class KoMatchViewModel
    {
        public Guid FixtureId { get; set; }
        public int ApiFixtureId { get; set; }
        public DateTimeOffset KickoffUtc { get; set; }

        public string? HomeTeam { get; set; }
        public string? AwayTeam { get; set; }
        public string HomeLogoUrl { get; set; } = "";
        public string AwayLogoUrl { get; set; } = "";
        public Guid? HomeTeamId { get; set; }
        public Guid? AwayTeamId { get; set; }

        public string? HomeQualificationCode { get; set; }
        public string? AwayQualificationCode { get; set; }

        public int? PredictedHomeGoals { get; set; }
        public int? PredictedAwayGoals { get; set; }
        public Guid? PredictedWinnerId { get; set; }

        // Beide Teams bekannt?
        public bool IsReady => HomeTeamId.HasValue && AwayTeamId.HasValue;
        // Bereits getippt?
        public bool IsTipped => PredictedWinnerId.HasValue;
    }
}
