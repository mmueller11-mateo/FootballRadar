namespace FootballRadar.DataCollector.FootballAPI.Models.Fixture
{
    public class FixtureLeague
    {
        public int Id { get; set; }
        public int Season { get; set; }
        public string Round { get; set; } = default!;
    }
}
