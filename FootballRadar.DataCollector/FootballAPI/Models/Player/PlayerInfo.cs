namespace FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Player
{
    public class PlayerInfo
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Firstname { get; set; }
        public required string Lastname { get; set; }
        public required PlayerBirth Birth { get; set; }
        public required string Nationality { get; set; }
        public required string Photo { get; set; }
    }
}
