namespace FootballRadar.Admin.WebApp.Models
{
    public class WmMatchAdminViewModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Phase { get; set; } = "";
        public string? WmGroup { get; set; }
        public string HomeTeam { get; set; } = "";
        public string AwayTeam { get; set; } = "";
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public bool IsKnockout { get; set; }
        public bool HasResult { get; set; }
    }
}
