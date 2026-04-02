namespace FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Team
{
    public class TeamInfo
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Logo { get; set; }
        public required string Code { get; set; }
    }
}
