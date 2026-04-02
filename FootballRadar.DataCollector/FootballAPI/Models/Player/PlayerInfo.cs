namespace FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Player
{
    public class PlayerInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Nationality { get; set; }
        public string? Height { get; set; }
        public string? Weight { get; set; }
        public string? Photo { get; set; }
    }
}
