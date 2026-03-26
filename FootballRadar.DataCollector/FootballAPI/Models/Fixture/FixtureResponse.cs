namespace FootballRadar.DataCollector.FootballAPI.Models.Fixture
{
    public class FixtureResponse
    {
        public FixtureDetail Fixture { get; set; } = default!;
        public FixtureLeague League { get; set; } = default!;
        public FixtureTeams Teams { get; set; } = default!;
        public FixtureGoals Goals { get; set; } = default!;
    }

}
