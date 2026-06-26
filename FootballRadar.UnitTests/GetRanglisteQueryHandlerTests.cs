using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Services.Queries;
using FootballRadar.Business.Services.QueryHandlers;
using FootballRadar.Data.Repositories;
using Moq;

namespace FootballRadar.UnitTests
{
    [TestClass]
    public class GetRanglisteQueryHandlerTests
    {
        private static readonly Guid _user1Id = Guid.NewGuid();
        private static readonly Guid _user2Id = Guid.NewGuid();

        private static readonly Guid _match1Id = Guid.NewGuid();
        private static readonly Guid _match2Id = Guid.NewGuid();
        private static readonly Guid _match3Id = Guid.NewGuid();

        private static User MakeUser(Guid id, string name) => new()
        {
            Id = id,
            Name = name,
            Email = $"{name}@test.com",
            PasswordHash = "x"
        };

        private static WmTip MakeGroupTip(
            Guid userId,
            Guid matchId,
            int home,
            int away)
            => new()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                WmMatchId = matchId,
                HomeGoals = home,
                AwayGoals = away,
                IsKoMatch = false
            };

        private static WmTip MakeKoTip(
    Guid userId,
    Guid matchId,
    int home,
    int away)
    => new()
    {
        Id = Guid.NewGuid(),
        UserId = userId,
        WmMatchId = matchId,
        IsKoMatch = true,
        HomeGoals = home,
        AwayGoals = away,
    };

        private static Business.Entities.LeagueEntities.Match MakeMatch(
            Guid id,
            int? homeGoals,
            int? awayGoals,
            Guid? homeTeamId = null,
            Guid? awayTeamId = null)
            => new()
            {
                Id = id,
                HomeGoals = homeGoals,
                AwayGoals = awayGoals,
                HomeNationalTeamId = homeTeamId,
                AwayNationalTeamId = awayTeamId
            };

        private static GetRanglisteQueryHandler BuildHandler(
     IEnumerable<WmTip> tips,
     IEnumerable<Business.Entities.LeagueEntities.Match> matches,
     IEnumerable<User> users)
        {
            var tipRepo = new Mock<IWmTipRepository>();
            var matchRepo = new Mock<IMatchRepository>();
            var userRepo = new Mock<IUserRepository>();
            var bonustipRepo = new Mock<IBonusTipRepository>();

            tipRepo
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(tips);

            matchRepo
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(matches);

            userRepo
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);

            return new GetRanglisteQueryHandler(
                tipRepo.Object,
                userRepo.Object,
                matchRepo.Object,
                bonustipRepo.Object
                );
        }

        [TestMethod]
        public async Task GroupMatch_ExactResult_Returns4Points()
        {
            var tips = new[]
            {
                MakeGroupTip(_user1Id, _match1Id, 2, 1)
            };

            var matches = new[]
            {
                MakeMatch(_match1Id, 2, 1)
            };

            var users = new[]
            {
                MakeUser(_user1Id, "Alice")
            };

            var handler = BuildHandler(tips, matches, users);

            var result = (await handler.Handle(
                new GetRanglisteQuery(),
                CancellationToken.None)).ToList();

            Assert.AreEqual(4, result[0].TotalPoints);
        }

        [TestMethod]
        public async Task GroupMatch_CorrectGoalDifference_Returns3Points()
        {
            var tips = new[]
            {
                MakeGroupTip(_user1Id, _match1Id, 2, 1)
            };

            var matches = new[]
            {
                MakeMatch(_match1Id, 3, 2)
            };

            var users = new[]
            {
                MakeUser(_user1Id, "Alice")
            };

            var handler = BuildHandler(tips, matches, users);

            var result = (await handler.Handle(
                new GetRanglisteQuery(),
                CancellationToken.None)).ToList();

            Assert.AreEqual(3, result[0].TotalPoints);
        }

        [TestMethod]
        public async Task GroupMatch_CorrectTendency_Returns2Points()
        {
            var tips = new[]
            {
        MakeGroupTip(_user1Id, _match1Id, 2, 0)
    };

            var matches = new[]
            {
        MakeMatch(_match1Id, 4, 1)
    };

            var users = new[]
            {
        MakeUser(_user1Id, "Alice")
    };

            var handler = BuildHandler(tips, matches, users);

            var result = (await handler.Handle(
                new GetRanglisteQuery(),
                CancellationToken.None)).ToList();

            Assert.AreEqual(2, result[0].TotalPoints);
        }

        [TestMethod]
        public async Task GroupMatch_WrongTip_Returns0Points()
        {
            var tips = new[]
            {
                MakeGroupTip(_user1Id, _match1Id, 0, 2)
            };

            var matches = new[]
            {
                MakeMatch(_match1Id, 2, 0)
            };

            var users = new[]
            {
                MakeUser(_user1Id, "Alice")
            };

            var handler = BuildHandler(tips, matches, users);

            var result = (await handler.Handle(
                new GetRanglisteQuery(),
                CancellationToken.None)).ToList();

            Assert.AreEqual(0, result[0].TotalPoints);
        }

        [TestMethod]
        public async Task KoMatch_ExactResult_Returns4Points()
        {
            var tips = new[] { MakeKoTip(_user1Id, _match1Id, 2, 1) };
            var matches = new[] { MakeMatch(_match1Id, 2, 1) };
            var users = new[] { MakeUser(_user1Id, "Alice") };

            var result = (await BuildHandler(tips, matches, users)
                .Handle(new GetRanglisteQuery(), CancellationToken.None)).ToList();

            Assert.AreEqual(4, result[0].TotalPoints);
        }

        [TestMethod]
        public async Task KoMatch_CorrectDifference_Returns3Points()
        {
            var tips = new[] { MakeKoTip(_user1Id, _match1Id, 2, 1) };
            var matches = new[] { MakeMatch(_match1Id, 3, 2) };
            var users = new[] { MakeUser(_user1Id, "Alice") };

            var result = (await BuildHandler(tips, matches, users)
                .Handle(new GetRanglisteQuery(), CancellationToken.None)).ToList();

            Assert.AreEqual(3, result[0].TotalPoints);
        }

        [TestMethod]
        public async Task KoMatch_CorrectTendency_Returns2Points()
        {
            var tips = new[] { MakeKoTip(_user1Id, _match1Id, 3, 1) };
            var matches = new[] { MakeMatch(_match1Id, 1, 0) };
            var users = new[] { MakeUser(_user1Id, "Alice") };

            var result = (await BuildHandler(tips, matches, users)
                .Handle(new GetRanglisteQuery(), CancellationToken.None)).ToList();

            Assert.AreEqual(2, result[0].TotalPoints);
        }

        [TestMethod]
        public async Task KoMatch_WrongTendency_Returns0Points()
        {
            var tips = new[] { MakeKoTip(_user1Id, _match1Id, 0, 2) };
            var matches = new[] { MakeMatch(_match1Id, 2, 0) };
            var users = new[] { MakeUser(_user1Id, "Alice") };

            var result = (await BuildHandler(tips, matches, users)
                .Handle(new GetRanglisteQuery(), CancellationToken.None)).ToList();

            Assert.AreEqual(0, result[0].TotalPoints);
        }

        [TestMethod]
        public async Task KoMatch_NotPlayed_Returns0Points()
        {
            var tips = new[] { MakeKoTip(_user1Id, _match1Id, 2, 1) };
            var matches = new[] { MakeMatch(_match1Id, null, null) };
            var users = new[] { MakeUser(_user1Id, "Alice") };

            var result = (await BuildHandler(tips, matches, users)
                .Handle(new GetRanglisteQuery(), CancellationToken.None)).ToList();

            Assert.AreEqual(0, result[0].TotalPoints);
        }

        [TestMethod]
        public async Task GroupAndKoPoints_AreSummedCorrectly()
        {
            var tips = new WmTip[]
            {
        MakeGroupTip(_user1Id, _match1Id, 2, 1), // exakt → 4 Punkte
        MakeKoTip(_user1Id, _match2Id, 2, 0)     // richtige Differenz → 3 Punkte
            };

            var matches = new[]
            {
        MakeMatch(_match1Id, 2, 1),
        MakeMatch(_match2Id, 3, 1)
    };

            var users = new[] { MakeUser(_user1Id, "Alice") };

            var result = (await BuildHandler(tips, matches, users)
                .Handle(new GetRanglisteQuery(), CancellationToken.None)).ToList();

            Assert.AreEqual(7, result[0].TotalPoints);
        }

        [TestMethod]
        public async Task TwoUsers_AreRankedCorrectly()
        {
            var tips = new WmTip[]
            {
        MakeGroupTip(_user1Id, _match1Id, 2, 1), // exakt → 4
        MakeKoTip(_user1Id, _match2Id, 2, 0),    // richtige Differenz → 3

        MakeGroupTip(_user2Id, _match1Id, 0, 0), // falsch → 0
        MakeKoTip(_user2Id, _match2Id, 0, 1)     // falsche Tendenz → 0
            };

            var matches = new[]
            {
        MakeMatch(_match1Id, 2, 1),
        MakeMatch(_match2Id, 3, 1)
    };

            var users = new[]
            {
        MakeUser(_user1Id, "Alice"),
        MakeUser(_user2Id, "Bob")
    };

            var result = (await BuildHandler(tips, matches, users)
                .Handle(new GetRanglisteQuery(), CancellationToken.None)).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Alice", result[0].TipperName);
            Assert.AreEqual(7, result[0].TotalPoints);
            Assert.AreEqual("Bob", result[1].TipperName);
            Assert.AreEqual(0, result[1].TotalPoints);
        }

        [TestMethod]
        public async Task UnknownUser_FallsBackToUnbekannt()
        {
            var tips = new[]
            {
                MakeGroupTip(_user1Id, _match1Id, 2, 1)
            };

            var matches = new[]
            {
                MakeMatch(_match1Id, 2, 1)
            };

            var tipRepo = new Mock<IWmTipRepository>();
            var matchRepo = new Mock<IMatchRepository>();
            var userRepo = new Mock<IUserRepository>();
            var bonustipRepo = new Mock<IBonusTipRepository>();
            tipRepo
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(tips);

            matchRepo
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(matches);

            userRepo
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            var handler = new GetRanglisteQueryHandler(
                tipRepo.Object,
                userRepo.Object,
                matchRepo.Object,
                bonustipRepo.Object);

            var result = (await handler.Handle(
                new GetRanglisteQuery(),
                CancellationToken.None)).ToList();

            Assert.AreEqual("Unbekannt", result[0].TipperName);
        }
    }
}