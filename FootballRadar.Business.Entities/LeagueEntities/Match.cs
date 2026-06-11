using FootballRadar.Business.Entities.Enums;

namespace FootballRadar.Business.Entities.LeagueEntities
{
    public class Match
    {
        public Guid Id { get; set; }
        public int ApiFixtureId { get; set; }
        public DateTimeOffset Date { get; set; }
        public int Season { get; set; }
        public string? Round { get; set; }
        public string? Status { get; set; }
        public Guid LeagueId { get; set; }
        public Guid? HomeTeamId { get; set; }
        public Guid? HomeNationalTeamId { get; set; }
        public Guid? AwayTeamId { get; set; }
        public Guid? AwayNationalTeamId { get; set; }
        public int? HomeGoals { get; set; }
        public int? AwayGoals { get; set; }
        public WmPhase? WmPhase { get; set; }
        public string? WmGroup { get; set; }
        public string? HomeQualificationCode { get; set; }
        public string? AwayQualificationCode { get; set; }
        public Guid? KoWinnerTeamId { get; set; }
    }
}
