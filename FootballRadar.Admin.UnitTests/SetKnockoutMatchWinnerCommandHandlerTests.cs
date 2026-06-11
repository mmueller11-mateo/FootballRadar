using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.CommandHandlers.Create;
using FootballRadar.Admin.Business.Services.Commands.Create;
using FootballRadar.Business.Entities.Enums;
using Moq;

namespace FootballRadar.Admin.Tests.CommandHandlers
{
    [TestClass]
    public class SetKnockoutMatchWinnerCommandHandlerTests
    {
        private Mock<IMatchRepository> matchRepoMock = null!;
        private Mock<IWmTipRepository> wmTipRepoMock = null!;
        private SetKnockoutMatchWinnerCommandHandler handler = null!;

        private static readonly Guid MatchId = Guid.NewGuid();

        [TestInitialize]
        public void Setup()
        {
            matchRepoMock = new Mock<IMatchRepository>();
            wmTipRepoMock = new Mock<IWmTipRepository>();
            handler = new SetKnockoutMatchWinnerCommandHandler(
                matchRepoMock.Object,
                wmTipRepoMock.Object);
        }

        // ===== MATCH NOT FOUND =====

        [TestMethod]
        public async Task Handle_MatchNotFound_DoesNothing()
        {
            matchRepoMock
                .Setup(r => r.GetByIdAsync(MatchId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((FootballRadar.Business.Entities.LeagueEntities.Match?)null);

            await handler.Handle(new SetKnockoutMatchWinnerCommand
            {
                MatchId = MatchId,
                Winner = "home"
            }, CancellationToken.None);

            matchRepoMock.Verify(r => r.UpdateAsync(It.IsAny<FootballRadar.Business.Entities.LeagueEntities.Match>(), It.IsAny<CancellationToken>()), Times.Never);
            wmTipRepoMock.Verify(r => r.GetByMatchIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        // ===== RESULTAT SPEICHERN: HEIMSIEG =====

        [TestMethod]
        public async Task Handle_HomeWinner_SavesResult1to0()
        {
            var match = CreateMatch();
            SetupMocks(match, new List<WmTip>());

            await handler.Handle(new SetKnockoutMatchWinnerCommand
            {
                MatchId = MatchId,
                Winner = "home"
            }, CancellationToken.None);

            Assert.AreEqual(1, match.HomeGoals);
            Assert.AreEqual(0, match.AwayGoals);
            Assert.AreEqual("FT", match.Status);
        }

        // ===== RESULTAT SPEICHERN: AUSWÄRTSSIEG =====

        [TestMethod]
        public async Task Handle_AwayWinner_SavesResult0to1()
        {
            var match = CreateMatch();
            SetupMocks(match, new List<WmTip>());

            await handler.Handle(new SetKnockoutMatchWinnerCommand
            {
                MatchId = MatchId,
                Winner = "away"
            }, CancellationToken.None);

            Assert.AreEqual(0, match.HomeGoals);
            Assert.AreEqual(1, match.AwayGoals);
            Assert.AreEqual("FT", match.Status);
        }

        // ===== PUNKTE: RICHTIGER SIEGER HEIM =====

        [TestMethod]
        public async Task Handle_CorrectHomeWinnerTip_Awards3Points()
        {
            var match = CreateMatch();
            var tip = CreateTip(homeGoals: 1, awayGoals: 0); // Heimsieg getippt

            SetupMocks(match, tip);

            await handler.Handle(new SetKnockoutMatchWinnerCommand
            {
                MatchId = MatchId,
                Winner = "home"
            }, CancellationToken.None);

            Assert.AreEqual(3, tip.Points);
        }

        // ===== PUNKTE: RICHTIGER SIEGER AUSWÄRTS =====

        [TestMethod]
        public async Task Handle_CorrectAwayWinnerTip_Awards3Points()
        {
            var match = CreateMatch();
            var tip = CreateTip(homeGoals: 0, awayGoals: 1); // Auswärtssieg getippt

            SetupMocks(match, tip);

            await handler.Handle(new SetKnockoutMatchWinnerCommand
            {
                MatchId = MatchId,
                Winner = "away"
            }, CancellationToken.None);

            Assert.AreEqual(3, tip.Points);
        }

        // ===== PUNKTE: FALSCHER SIEGER =====

        [TestMethod]
        public async Task Handle_WrongWinnerTip_Awards0Points()
        {
            var match = CreateMatch();
            var tip = CreateTip(homeGoals: 1, awayGoals: 0); // Heimsieg getippt

            SetupMocks(match, tip);

            await handler.Handle(new SetKnockoutMatchWinnerCommand
            {
                MatchId = MatchId,
                Winner = "away" // Auswärtssieg effektiv
            }, CancellationToken.None);

            Assert.AreEqual(0, tip.Points);
        }

        // ===== MEHRERE TIPPS =====

        [TestMethod]
        public async Task Handle_MultipleTips_EachEvaluatedCorrectly()
        {
            var match = CreateMatch();
            var tipCorrect = CreateTip(homeGoals: 1, awayGoals: 0); // Heimsieg getippt
            var tipWrong = CreateTip(homeGoals: 0, awayGoals: 1); // Auswärtssieg getippt

            matchRepoMock
                .Setup(r => r.GetByIdAsync(MatchId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(match);
            wmTipRepoMock
                .Setup(r => r.GetByMatchIdAsync(MatchId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<WmTip> { tipCorrect, tipWrong });

            await handler.Handle(new SetKnockoutMatchWinnerCommand
            {
                MatchId = MatchId,
                Winner = "home"
            }, CancellationToken.None);

            Assert.AreEqual(3, tipCorrect.Points);
            Assert.AreEqual(0, tipWrong.Points);

            wmTipRepoMock.Verify(
                r => r.UpdateAsync(It.IsAny<WmTip>(), It.IsAny<CancellationToken>()),
                Times.Exactly(2));
        }

        // ===== UPDATE WIRD PRO TIP AUFGERUFEN =====

        [TestMethod]
        public async Task Handle_UpdateCalledForEachTip()
        {
            var match = CreateMatch();
            var tips = new List<WmTip>
            {
                CreateTip(homeGoals: 1, awayGoals: 0),
                CreateTip(homeGoals: 1, awayGoals: 0),
                CreateTip(homeGoals: 0, awayGoals: 1)
            };

            matchRepoMock
                .Setup(r => r.GetByIdAsync(MatchId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(match);
            wmTipRepoMock
                .Setup(r => r.GetByMatchIdAsync(MatchId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tips);

            await handler.Handle(new SetKnockoutMatchWinnerCommand
            {
                MatchId = MatchId,
                Winner = "home"
            }, CancellationToken.None);

            wmTipRepoMock.Verify(
                r => r.UpdateAsync(It.IsAny<WmTip>(), It.IsAny<CancellationToken>()),
                Times.Exactly(3));
        }

        // ===== HELPERS =====

        private static FootballRadar.Business.Entities.LeagueEntities.Match CreateMatch() => new FootballRadar.Business.Entities.LeagueEntities.Match
        {
            Id = MatchId,
            WmPhase = WmPhase.RoundOf16,
            LeagueId = Guid.NewGuid()
        };

        private static WmTip CreateTip(int homeGoals, int awayGoals) => new WmTip
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            WmMatchId = MatchId,
            HomeGoals = homeGoals,
            AwayGoals = awayGoals
        };

        private void SetupMocks(FootballRadar.Business.Entities.LeagueEntities.Match match, WmTip tip) =>
            SetupMocks(match, new List<WmTip> { tip });

        private void SetupMocks(FootballRadar.Business.Entities.LeagueEntities.Match match, List<WmTip> tips)
        {
            matchRepoMock
                .Setup(r => r.GetByIdAsync(MatchId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(match);
            wmTipRepoMock
                .Setup(r => r.GetByMatchIdAsync(MatchId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tips);
        }
    }
}