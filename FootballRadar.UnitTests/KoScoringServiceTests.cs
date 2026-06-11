using FootballRadar.Business.Entities.LeagueEntities;
using FootballRadar.Business.Services.TippSpiel;

namespace FootballRadar.UnitTests
{
    [TestClass]
    public class KoScoringServiceTests
    {
        [TestMethod]
        public void Calculate_Returns_0_When_Match_Has_No_Result()
        {
            // Arrange
            var tip = new WmTip
            {
                PredictedWinnerId = Guid.NewGuid()
            };

            var match = new Match
            {
                HomeGoals = null,
                AwayGoals = null,
                HomeNationalTeamId = Guid.NewGuid(),
                AwayNationalTeamId = Guid.NewGuid()
            };

            // Act
            var points = KoScoringService.Calculate(tip, match);

            // Assert
            Assert.AreEqual(0, points);
        }

        [TestMethod]
        public void Calculate_Returns_3_When_HomeTeam_Winner_Is_Correct()
        {
            // Arrange
            var homeId = Guid.NewGuid();
            var awayId = Guid.NewGuid();

            var tip = new WmTip
            {
                PredictedWinnerId = homeId
            };

            var match = new Match
            {
                HomeGoals = 2,
                AwayGoals = 1,
                HomeNationalTeamId = homeId,
                AwayNationalTeamId = awayId
            };

            // Act
            var points = KoScoringService.Calculate(tip, match);

            // Assert
            Assert.AreEqual(3, points);
        }

        [TestMethod]
        public void Calculate_Returns_0_When_Winner_Is_Wrong()
        {
            // Arrange
            var homeId = Guid.NewGuid();
            var awayId = Guid.NewGuid();

            var tip = new WmTip
            {
                PredictedWinnerId = homeId
            };

            var match = new Match
            {
                HomeGoals = 0,
                AwayGoals = 3,
                HomeNationalTeamId = homeId,
                AwayNationalTeamId = awayId
            };

            // Act
            var points = KoScoringService.Calculate(tip, match);

            // Assert
            Assert.AreEqual(0, points);
        }

        [TestMethod]
        public void Calculate_Returns_0_When_Draw()
        {
            // Arrange
            var homeId = Guid.NewGuid();
            var awayId = Guid.NewGuid();

            var tip = new WmTip
            {
                PredictedWinnerId = homeId
            };

            var match = new Match
            {
                HomeGoals = 1,
                AwayGoals = 1,
                HomeNationalTeamId = homeId,
                AwayNationalTeamId = awayId
            };

            // Act
            var points = KoScoringService.Calculate(tip, match);

            // Assert
            Assert.AreEqual(0, points);
        }
    }
}