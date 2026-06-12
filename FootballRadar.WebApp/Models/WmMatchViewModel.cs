using FootballRadar.Business.Entities.Enums;

namespace FootballRadar.WebApp.Models
{
    public class WmMatchViewModel
    {
        public Guid Id { get; set; }

        public string HomeTeam { get; set; } = "";
        public string AwayTeam { get; set; } = "";
        public string HomeLogoUrl { get; set; } = "";
        public string AwayLogoUrl { get; set; } = "";
        public DateTimeOffset KickoffUtc { get; set; }
        public string? WmGroup { get; set; }
        public WmPhase WmPhase { get; set; }
        public int? HomeGoals { get; set; }
        public int? AwayGoals { get; set; }
    }
}
