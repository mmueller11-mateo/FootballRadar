namespace FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Fixture
{
    public class FixtureDetail
    {
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public FixtureStatus Status { get; set; } = default!;
    }

}
