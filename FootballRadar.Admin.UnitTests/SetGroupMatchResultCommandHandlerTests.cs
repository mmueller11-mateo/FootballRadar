using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.CommandHandlers.Create;
using FootballRadar.Admin.Business.Services.Commands.Create;
using FootballRadar.Business.Entities.Enums;
using Moq;

namespace FootballRadar.Admin.Tests.CommandHandlers
{
    [TestClass]
    public class SetGroupMatchResultCommandHandlerTests
    {
        private Mock<IMatchRepository> matchRepoMock = null!;
        private Mock<IWmTipRepository> wmTipRepoMock = null!;
        private SetGroupMatchResultCommandHandler handler = null!;

        private static readonly Guid MatchId = Guid.NewGuid();

        [TestInitialize]
        public void Setup()
        {
            matchRepoMock = new Mock<IMatchRepository>();
            wmTipRepoMock = new Mock<IWmTipRepository>();
            handler = new SetGroupMatchResultCommandHandler(
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

            await handler.Handle(new SetGroupMatchResultCommand
            {
                MatchId = MatchId,
                HomeGoals = 2,
                AwayGoals = 1
            }, CancellationToken.None);

            matchRepoMock.Verify(r => r.UpdateAsync(It.IsAny<FootballRadar.Business.Entities.LeagueEntities.Match>(), It.IsAny<CancellationToken>()), Times.Never);
            wmTipRepoMock.Verify(r => r.GetByMatchIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        // ===== RESULTAT SPEICHERN =====

        [TestMethod]
        public async Task Handle_SavesResultAndStatusFT()
        {
            var match = CreateMatch();
            matchRepoMock
                .Setup(r => r.GetByIdAsync(MatchId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(match);
            wmTipRepoMock
                .Setup(r => r.GetByMatchIdAsync(MatchId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<WmTip>());

            await handler.Handle(new SetGroupMatchResultCommand
            {
                MatchId = MatchId,
                HomeGoals = 3,
                AwayGoals = 1
            }, CancellationToken.None);

            Assert.AreEqual(3, match.HomeGoals);
            Assert.AreEqual(1, match.AwayGoals);
            Assert.AreEqual("FT", match.Status);
            matchRepoMock.Verify(r => r.UpdateAsync(match, It.IsAny<CancellationToken>()), Times.Once);
        }

        // ===== PUNKTE: EXAKTES ERGEBNIS =====

        [TestMethod]
        public async Task Handle_ExactTip_Awards3Points()
        {
            var match = CreateMatch();
            var tip = CreateTip(homeGoals: 2, awayGoals: 1);

            SetupMocks(match, tip);

            await handler.Handle(new SetGroupMatchResultCommand
            {
                MatchId = MatchId,
                HomeGoals = 2,
                AwayGoals = 1
            }, CancellationToken.None);

            Assert.AreEqual(3, tip.Points);
        }

        // ===== PUNKTE: RICHTIGE TENDENZ (HEIMSIEG) =====

        [TestMethod]
        public async Task Handle_CorrectTendency_HomWin_Awards1Point()
        {
            var match = CreateMatch();
            var tip = CreateTip(homeGoals: 3, awayGoals: 0); // Heimsieg getippt

            SetupMocks(match, tip);

            await handler.Handle(new SetGroupMatchResultCommand
            {
                MatchId = MatchId,
                HomeGoals = 1, // Auch Heimsieg, aber anderes Ergebnis
                AwayGoals = 0
            }, CancellationToken.None);

            Assert.AreEqual(1, tip.Points);
        }

        // ===== PUNKTE: RICHTIGE TENDENZ (UNENTSCHIEDEN) =====

        [TestMethod]
        public async Task Handle_CorrectTendency_Draw_Awards1Point()
        {
            var match = CreateMatch();
            var tip = CreateTip(homeGoals: 1, awayGoals: 1);

            SetupMocks(match, tip);

            await handler.Handle(new SetGroupMatchResultCommand
            {
                MatchId = MatchId,
                HomeGoals = 2,
                AwayGoals = 2
            }, CancellationToken.None);

            Assert.AreEqual(1, tip.Points);
        }

        // ===== PUNKTE: RICHTIGE TENDENZ (AUSWÄRTSSIEG) =====

        [TestMethod]
        public async Task Handle_CorrectTendency_AwayWin_Awards1Point()
        {
            var match = CreateMatch();
            var tip = CreateTip(homeGoals: 0, awayGoals: 2);

            SetupMocks(match, tip);

            await handler.Handle(new SetGroupMatchResultCommand
            {
                MatchId = MatchId,
                HomeGoals = 1,
                AwayGoals = 3
            }, CancellationToken.None);

            Assert.AreEqual(1, tip.Points);
        }

        // ===== PUNKTE: FALSCHE TENDENZ =====

        [TestMethod]
        public async Task Handle_WrongTendency_Awards0Points()
        {
            var match = CreateMatch();
            var tip = CreateTip(homeGoals: 2, awayGoals: 0); // Heimsieg getippt

            SetupMocks(match, tip);

            await handler.Handle(new SetGroupMatchResultCommand
            {
                MatchId = MatchId,
                HomeGoals = 0, // Auswärtssieg
                AwayGoals = 1
            }, CancellationToken.None);

            Assert.AreEqual(0, tip.Points);
        }

        // ===== PUNKTE: UNENTSCHIEDEN GETIPPT, HEIMSIEG EFFEKTIV =====

        [TestMethod]
        public async Task Handle_TippedDraw_ActualHomeWin_Awards0Points()
        {
            var match = CreateMatch();
            var tip = CreateTip(homeGoals: 1, awayGoals: 1);

            SetupMocks(match, tip);

            await handler.Handle(new SetGroupMatchResultCommand
            {
                MatchId = MatchId,
                HomeGoals = 2,
                AwayGoals = 0
            }, CancellationToken.None);

            Assert.AreEqual(0, tip.Points);
        }

        // ===== MEHRERE TIPPS =====

        [TestMethod]
        public async Task Handle_MultipleTips_EachEvaluatedCorrectly()
        {
            var match = CreateMatch();
            var tipExact = CreateTip(homeGoals: 2, awayGoals: 1); // exakt
            var tipTendenz = CreateTip(homeGoals: 3, awayGoals: 0); // tendenz
            var tipFalsch = CreateTip(homeGoals: 0, awayGoals: 1); // falsch

            matchRepoMock
                .Setup(r => r.GetByIdAsync(MatchId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(match);
            wmTipRepoMock
                .Setup(r => r.GetByMatchIdAsync(MatchId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<WmTip> { tipExact, tipTendenz, tipFalsch });

            await handler.Handle(new SetGroupMatchResultCommand
            {
                MatchId = MatchId,
                HomeGoals = 2,
                AwayGoals = 1
            }, CancellationToken.None);

            Assert.AreEqual(3, tipExact.Points);
            Assert.AreEqual(1, tipTendenz.Points);
            Assert.AreEqual(0, tipFalsch.Points);

            wmTipRepoMock.Verify(
                r => r.UpdateAsync(It.IsAny<WmTip>(), It.IsAny<CancellationToken>()),
                Times.Exactly(3));
        }

        // ===== HELPERS =====

        private static FootballRadar.Business.Entities.LeagueEntities.Match CreateMatch() => new FootballRadar.Business.Entities.LeagueEntities.Match
        {
            Id = MatchId,
            WmPhase = WmPhase.Group,
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