using FootballRadar.Business.Entities.LeagueEntities;
using FootballRadar.Business.Services.TippSpiel;

namespace FootballRadar.UnitTests
{
    [TestClass]
    public class PredictionScoringServiceTests
    {
        private static WmTip Tip(int home, int away) => new()
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            WmMatchId = Guid.NewGuid(),
            HomeGoals = home,
            AwayGoals = away
        };

        private static Match Match(int? home, int? away) => new()
        {
            Id = Guid.NewGuid(),
            HomeGoals = home,
            AwayGoals = away
        };

        [TestMethod]
        public void Returns0_WhenMatchNotPlayed()
            => Assert.AreEqual(0, PredictionScoringService.Calculate(Tip(2, 1), Match(null, null)));

        [DataRow(2, 1, 2, 1, 4, "Heimsieg exakt → 4")]
        [DataRow(0, 0, 0, 0, 4, "Unentschieden exakt → 4")]
        [DataRow(2, 1, 3, 2, 3, "Heimsieg Tordifferenz (+1) → 3")]
        [DataRow(1, 1, 2, 2, 3, "Unentschieden Tendenz → 3")]
        [DataRow(1, 0, 3, 1, 2, "Heimsieg Tendenz → 2")]
        [DataRow(0, 2, 1, 3, 4, "Auswärtssieg Tordifferenz (-2) → 4")]
        [DataRow(0, 1, 1, 3, 3, "Auswärtssieg Tendenz → 3")]
        [DataRow(1, 2, 1, 2, 5, "Auswärtssieg exakt → 5")]
        [DataRow(2, 0, 0, 2, 0, "Falsche Tendenz → 0")]
        [DataRow(1, 1, 2, 0, 0, "Unentschieden getippt, Heimsieg → 0")]
        public void Calculate_ReturnsExpectedPoints(int predH, int predA, int actH, int actA, int expected, string scenario)
            => Assert.AreEqual(expected,
                PredictionScoringService.Calculate(Tip(predH, predA), Match(actH, actA)));
    }
}