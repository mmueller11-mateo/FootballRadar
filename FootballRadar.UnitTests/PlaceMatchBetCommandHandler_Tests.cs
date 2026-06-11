using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Entities.Betting.Enums;
using FootballRadar.Business.Services;
using FootballRadar.Business.Services.CommandHandlers;
using FootballRadar.Business.Services.Commands;
using FootballRadar.Business.Services.MatchPredictionMarketRules;
using Moq;
using Match = FootballRadar.Business.Entities.LeagueEntities.Match;

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
            var mockBetRepository = new Mock<IBetRepository>();
            var mockPredictionMarketRepository = new Mock<IPredictionMarketRepository>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            var mockMatchPredictionRewardCalculator = new Mock<IMatchPredictionRewardCalculator>();
            var mockWalletRepository = new Mock<IWalletRepository>();
            var mockRuleFactory = new Mock<IBetRuleFactory>();

            var command = new PlaceMatchBetCommand
            {
                UserId = Guid.NewGuid(),
                MatchId = Guid.NewGuid(),
                Credits = 100,
                Prediction = MatchPrediction.HomeWin
            };

            mockMatchRepository
                .Setup(x => x.GetByIdAsync(command.MatchId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Match?)null);

            var commandHandler = new PlaceMatchBetCommandHandler(
                mockPredictionMarketRepository.Object,
                mockMatchRepository.Object,
                mockMatchPredictionRewardCalculator.Object,
                mockWalletRepository.Object,
                mockBetRepository.Object,
                mockRuleFactory.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                commandHandler.Handle(command, CancellationToken.None));
        }

        [TestMethod]
        [Description("This test ensures that a predictionmarket gets created if none found")]
        public async Task ShouldCreatePredictionMarketIfNoneFound()
        {
            // Arrange
            var mockBetRepository = new Mock<IBetRepository>();
            var mockPredictionMarketRepository = new Mock<IPredictionMarketRepository>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            var mockMatchPredictionRewardCalculator = new Mock<IMatchPredictionRewardCalculator>();
            var mockWalletRepository = new Mock<IWalletRepository>();
            var mockRuleFactory = new Mock<IBetRuleFactory>();

            var matchId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var wallet = new Wallet(userId);
            wallet.Deposit(100);

            var command = new PlaceMatchBetCommand
            {
                UserId = userId,
                MatchId = matchId,
                Credits = 100,
                Prediction = MatchPrediction.HomeWin
            };

            mockMatchRepository
                .Setup(x => x.GetByIdAsync(matchId, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<Match?>(new Match { Id = matchId, Date = DateTimeOffset.UtcNow.AddHours(2) }));

            mockPredictionMarketRepository
                .Setup(x => x.FindForMatchAsync(matchId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((MatchPredictionMarket?)null);

            mockMatchPredictionRewardCalculator
                .Setup(x => x.CalculateReward(It.IsAny<Match>()))
                .ReturnsAsync(50);

            mockRuleFactory
                .Setup(x => x.CreateRulesAsync(It.IsAny<MatchPredictionContext>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<IPredictionMarketRule>());

            mockWalletRepository
                .Setup(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(wallet);

            var commandHandler = new PlaceMatchBetCommandHandler(
                mockPredictionMarketRepository.Object,
                mockMatchRepository.Object,
                mockMatchPredictionRewardCalculator.Object,
                mockWalletRepository.Object,
                mockBetRepository.Object,
                mockRuleFactory.Object);

            // Act
            await commandHandler.Handle(command, CancellationToken.None);

            // Assert
            mockPredictionMarketRepository.Verify(
                x => x.AddAsync(
                    It.Is<MatchPredictionMarket>(m => m.MatchId == matchId),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [TestMethod]
        [Description("This test ensures the rules are set to a bet")]
        public async Task ShouldSetRulesToBet()
        {
            // Arrange
            var mockBetRepository = new Mock<IBetRepository>();
            var mockPredictionMarketRepository = new Mock<IPredictionMarketRepository>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            var mockMatchPredictionRewardCalculator = new Mock<IMatchPredictionRewardCalculator>();
            var mockWalletRepository = new Mock<IWalletRepository>();
            var mockRuleFactory = new Mock<IBetRuleFactory>();

            var matchId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var wallet = new Wallet(userId);
            wallet.Deposit(100);

            var command = new PlaceMatchBetCommand
            {
                UserId = userId,
                MatchId = matchId,
                Credits = 100,
                Prediction = MatchPrediction.HomeWin
            };

            var rule1 = new Mock<IPredictionMarketRule>();
            rule1.Setup(r => r.Evaluate(It.IsAny<CancellationToken>())).ReturnsAsync(true);
            var rule2 = new Mock<IPredictionMarketRule>();
            rule2.Setup(r => r.Evaluate(It.IsAny<CancellationToken>())).ReturnsAsync(true);

            mockMatchRepository
                .Setup(x => x.GetByIdAsync(matchId, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<Match?>(new Match { Id = matchId, Date = DateTimeOffset.UtcNow.AddHours(2) }));

            mockPredictionMarketRepository
                .Setup(x => x.FindForMatchAsync(matchId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((MatchPredictionMarket?)null);

            mockMatchPredictionRewardCalculator
                .Setup(x => x.CalculateReward(It.IsAny<Match>()))
                .ReturnsAsync(50);

            mockRuleFactory
                .Setup(x => x.CreateRulesAsync(It.IsAny<MatchPredictionContext>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<IPredictionMarketRule> { rule1.Object, rule2.Object });

            mockWalletRepository
                .Setup(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(wallet);

            var commandHandler = new PlaceMatchBetCommandHandler(
                mockPredictionMarketRepository.Object,
                mockMatchRepository.Object,
                mockMatchPredictionRewardCalculator.Object,
                mockWalletRepository.Object,
                mockBetRepository.Object,
                mockRuleFactory.Object);

            // Act
            await commandHandler.Handle(command, CancellationToken.None);

            // Assert
            mockPredictionMarketRepository.Verify(
                x => x.AddAsync(
                    It.Is<MatchPredictionMarket>(m => m.Rules.Count == 2),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [TestMethod]
        [Description("This test ensures that a bet gets placed if all rules are satisfied")]
        public async Task ShouldPlaceBetIfRulesSatisfied()
        {
            // Arrange
            var mockBetRepository = new Mock<IBetRepository>();
            var mockPredictionMarketRepository = new Mock<IPredictionMarketRepository>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            var mockMatchPredictionRewardCalculator = new Mock<IMatchPredictionRewardCalculator>();
            var mockWalletRepository = new Mock<IWalletRepository>();
            var mockRuleFactory = new Mock<IBetRuleFactory>();

            var matchId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var wallet = new Wallet(userId);
            wallet.Deposit(100);

            var command = new PlaceMatchBetCommand
            {
                UserId = userId,
                MatchId = matchId,
                Credits = 100,
                Prediction = MatchPrediction.HomeWin
            };

            var rule1 = new Mock<IPredictionMarketRule>();
            rule1.Setup(r => r.Evaluate(It.IsAny<CancellationToken>())).ReturnsAsync(true);
            var rule2 = new Mock<IPredictionMarketRule>();
            rule2.Setup(r => r.Evaluate(It.IsAny<CancellationToken>())).ReturnsAsync(true);

            mockMatchRepository
                .Setup(x => x.GetByIdAsync(matchId, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<Match?>(new Match { Id = matchId, Date = DateTimeOffset.UtcNow.AddHours(2) }));

            mockPredictionMarketRepository
                .Setup(x => x.FindForMatchAsync(matchId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((MatchPredictionMarket?)null);

            mockMatchPredictionRewardCalculator
                .Setup(x => x.CalculateReward(It.IsAny<Match>()))
                .ReturnsAsync(50);

            mockRuleFactory
                .Setup(x => x.CreateRulesAsync(It.IsAny<MatchPredictionContext>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<IPredictionMarketRule> { rule1.Object, rule2.Object });

            mockWalletRepository
                .Setup(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(wallet);

            var commandHandler = new PlaceMatchBetCommandHandler(
                mockPredictionMarketRepository.Object,
                mockMatchRepository.Object,
                mockMatchPredictionRewardCalculator.Object,
                mockWalletRepository.Object,
                mockBetRepository.Object,
                mockRuleFactory.Object);

            // Act
            var result = await commandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(BetStatusCode.Accepted, result.Code);
            mockBetRepository.Verify(
                x => x.AddBetAsync(
                    It.Is<WinnerBet>(b => b.UserId == userId),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [TestMethod]
        [Description("This test ensures that a bet is rejected if any of the bet rules fails")]
        public async Task ShouldRejectBetIfAnyRuleFails()
        {
            // Arrange
            var mockBetRepository = new Mock<IBetRepository>();
            var mockPredictionMarketRepository = new Mock<IPredictionMarketRepository>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            var mockMatchPredictionRewardCalculator = new Mock<IMatchPredictionRewardCalculator>();
            var mockWalletRepository = new Mock<IWalletRepository>();
            var mockRuleFactory = new Mock<IBetRuleFactory>();

            var matchId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var wallet = new Wallet(userId);
            wallet.Deposit(100);

            var command = new PlaceMatchBetCommand
            {
                UserId = userId,
                MatchId = matchId,
                Credits = 100,
                Prediction = MatchPrediction.HomeWin
            };

            var passingRule = new Mock<IPredictionMarketRule>();
            passingRule.Setup(r => r.Evaluate(It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var failingRule = new Mock<IPredictionMarketRule>();
            failingRule.Setup(r => r.Evaluate(It.IsAny<CancellationToken>())).ReturnsAsync(false);
            failingRule.Setup(r => r.ErrorMessage).Returns("Betting is not allowed after match end");

            mockMatchRepository
                .Setup(x => x.GetByIdAsync(matchId, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<Match?>(new Match { Id = matchId, Date = DateTimeOffset.UtcNow.AddHours(2) }));

            mockPredictionMarketRepository
                .Setup(x => x.FindForMatchAsync(matchId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((MatchPredictionMarket?)null);

            mockMatchPredictionRewardCalculator
                .Setup(x => x.CalculateReward(It.IsAny<Match>()))
                .ReturnsAsync(50);

            mockRuleFactory
                .Setup(x => x.CreateRulesAsync(It.IsAny<MatchPredictionContext>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<IPredictionMarketRule> { passingRule.Object, failingRule.Object });

            mockWalletRepository
                .Setup(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(wallet);

            var commandHandler = new PlaceMatchBetCommandHandler(
                mockPredictionMarketRepository.Object,
                mockMatchRepository.Object,
                mockMatchPredictionRewardCalculator.Object,
                mockWalletRepository.Object,
                mockBetRepository.Object,
                mockRuleFactory.Object);

            // Act
            var result = await commandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(BetStatusCode.Rejected, result.Code);
            Assert.AreEqual("Betting is not allowed after match end", result.ErrorMessage);
            mockBetRepository.Verify(
                x => x.AddBetAsync(It.IsAny<WinnerBet>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [TestMethod]
        [Description("This test ensures that a bet cannot be placed after the match has started")]
        public async Task ShouldRejectBetIfMatchAlreadyStarted()
        {
            // Arrange
            var mockBetRepository = new Mock<IBetRepository>();
            var mockPredictionMarketRepository = new Mock<IPredictionMarketRepository>();
            var mockMatchRepository = new Mock<IMatchRepository>();
            var mockMatchPredictionRewardCalculator = new Mock<IMatchPredictionRewardCalculator>();
            var mockWalletRepository = new Mock<IWalletRepository>();
            var mockRuleFactory = new Mock<IBetRuleFactory>();

            var matchId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var wallet = new Wallet(userId);
            wallet.Deposit(100);

            var command = new PlaceMatchBetCommand
            {
                UserId = userId,
                MatchId = matchId,
                Credits = 100,
                Prediction = MatchPrediction.HomeWin
            };

            // Match liegt in der Vergangenheit
            mockMatchRepository
                .Setup(x => x.GetByIdAsync(matchId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Match { Id = matchId, Date = DateTimeOffset.UtcNow.AddHours(-1) });

            mockPredictionMarketRepository
                .Setup(x => x.FindForMatchAsync(matchId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((MatchPredictionMarket?)null);

            mockMatchPredictionRewardCalculator
                .Setup(x => x.CalculateReward(It.IsAny<Match>()))
                .ReturnsAsync(50);

            var matchStartedRule = new Mock<IPredictionMarketRule>();
            matchStartedRule.Setup(r => r.Evaluate(It.IsAny<CancellationToken>())).ReturnsAsync(false);
            matchStartedRule.Setup(r => r.ErrorMessage).Returns("Betting is not allowed after match start");

            mockRuleFactory
                .Setup(x => x.CreateRulesAsync(It.IsAny<MatchPredictionContext>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<IPredictionMarketRule> { matchStartedRule.Object });

            mockWalletRepository
                .Setup(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(wallet);

            var commandHandler = new PlaceMatchBetCommandHandler(
                mockPredictionMarketRepository.Object,
                mockMatchRepository.Object,
                mockMatchPredictionRewardCalculator.Object,
                mockWalletRepository.Object,
                mockBetRepository.Object,
                mockRuleFactory.Object);

            // Act
            var result = await commandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(BetStatusCode.Rejected, result.Code);
            Assert.AreEqual("Betting is not allowed after match start", result.ErrorMessage);
            mockBetRepository.Verify(
                x => x.AddBetAsync(It.IsAny<WinnerBet>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}