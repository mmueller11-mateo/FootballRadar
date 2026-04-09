namespace FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Player
{
    public class PlayerInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public PlayerBirth? Birth { get; set; }
        public string Nationality { get; set; }
        public string? Photo { get; set; }
    }
}
