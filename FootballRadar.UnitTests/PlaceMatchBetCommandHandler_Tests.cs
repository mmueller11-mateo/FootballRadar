using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting.Enums;
using FootballRadar.Business.Services;
using FootballRadar.Business.Services.CommandHandlers;
using FootballRadar.Business.Services.Commands;
using Moq;

namespace FootballRadar.UnitTests
{
    [TestClass]
    public sealed class PlaceMatchBetCommandHandler_Tests
    {
        [TestMethod]
        [Description("This test ensures that no bet can be placed for unknown matches")]
        public async Task ShouldThrowExceptionIfMatchNotFound()
        {
            // Arrange
            var mockBetrepository = new Mock<IBetRepository>();
            var mockPredictionMarketRepository = new Mock<IPredictionMarketRepository>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            var mockMatchPredictionRewardCalculator = new Mock<IMatchPredictionRewardCalculator>();
            var mockWalletRepository = new Mock<IWalletRepository>();
            var command = new PlaceMatchBetCommand
            {
                UserId = Guid.NewGuid(),
                MatchId = Guid.NewGuid(),
                Credits = 100,
                Prediction = MatchPrediction.HomeWin
            };
            mockMatchRepository
            .Setup(x => x.GetByIdAsync(command.MatchId, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<FootballRadar.Business.Entities.LeagueEntities.Match?>(null));

            // Act & Assert
            var commandHandler = new PlaceMatchBetCommandHandler(
                mockBetrepository.Object,
                mockPredictionMarketRepository.Object,
                mockMatchRepository.Object,
                mockMatchPredictionRewardCalculator.Object,
                mockWalletRepository.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
            commandHandler.Handle(command, CancellationToken.None));
        }

        [TestMethod]
        [Description("This test ensures that a predictionmarket gets created if none found")]
        public async Task ShouldCreatePredictionMarketIfNoneFound()
        {

        }

        [TestMethod]
        [Description("This test ensures the rules are set to a bet")]
        public async Task ShouldSetRulesToBet()
        {
        }

        [TestMethod]
        [Description("This test ensures that a bet gets placed if all rules are satisfied")]
        public async Task ShouldPlaceBetIfRulesSatisfied()
        {

        }

        [TestMethod]
        [Description("This test ensures that a bet is rejected if any of the betrules fails")]
        public async Task ShouldRejectBetIfAnyRuleFails()
        {

        }
    }
}
