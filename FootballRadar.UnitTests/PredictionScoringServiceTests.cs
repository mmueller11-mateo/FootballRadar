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

        [TestMethod]
        [DataRow(2, 1, 2, 1, 4, "Exact result")]
        [DataRow(0, 0, 0, 0, 4, "Exact draw")]
        [DataRow(2, 1, 3, 2, 3, "Correct diff (+1), wrong score")]
        [DataRow(1, 1, 2, 2, 2, "Correct tendency (draw), wrong score → 2pts, no diff tier for draws")]
        [DataRow(2, 1, 3, 0, 2, "Home win correct, diff wrong")]
        [DataRow(1, 2, 0, 3, 2, "Away win correct, diff wrong")]
        [DataRow(2, 0, 0, 2, 0, "Completely wrong")]
        [DataRow(1, 1, 2, 0, 0, "Predicted draw, actual home win")]
        public void Calculate_ReturnsExpectedPoints(int predH, int predA, int actH, int actA, int expected, string scenario)
            => Assert.AreEqual(expected,
                PredictionScoringService.Calculate(Tip(predH, predA), Match(actH, actA)));
    }
}