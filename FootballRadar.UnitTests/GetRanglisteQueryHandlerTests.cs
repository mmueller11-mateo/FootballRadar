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
            Guid predictedWinnerId)
            => new()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                WmMatchId = matchId,
                IsKoMatch = true,
                PredictedWinnerId = predictedWinnerId
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
        public async Task KoMatch_CorrectWinner_Returns3Points()
        {
            var homeId = Guid.NewGuid();
            var awayId = Guid.NewGuid();

            var tips = new[]
            {
                MakeKoTip(_user1Id, _match1Id, homeId)
            };

            var matches = new[]
            {
                MakeMatch(_match1Id, 2, 1, homeId, awayId)
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
        public async Task KoMatch_WrongWinner_Returns0Points()
        {
            var homeId = Guid.NewGuid();
            var awayId = Guid.NewGuid();

            var tips = new[]
            {
                MakeKoTip(_user1Id, _match1Id, awayId)
            };

            var matches = new[]
            {
                MakeMatch(_match1Id, 3, 1, homeId, awayId)
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
        public async Task KoMatch_NotPlayed_Returns0Points()
        {
            var homeId = Guid.NewGuid();
            var awayId = Guid.NewGuid();

            var tips = new[]
            {
                MakeKoTip(_user1Id, _match1Id, homeId)
            };

            var matches = new[]
            {
                MakeMatch(_match1Id, null, null, homeId, awayId)
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
        public async Task GroupAndKoPoints_AreSummedCorrectly()
        {
            var home1 = Guid.NewGuid();
            var away1 = Guid.NewGuid();

            var home2 = Guid.NewGuid();
            var away2 = Guid.NewGuid();

            var tips = new WmTip[]
            {
                MakeGroupTip(_user1Id, _match1Id, 2, 1),
                MakeKoTip(_user1Id, _match2Id, home2)
            };

            var matches = new[]
            {
                MakeMatch(_match1Id, 2, 1, home1, away1),
                MakeMatch(_match2Id, 1, 0, home2, away2)
            };

            var users = new[]
            {
                MakeUser(_user1Id, "Alice")
            };

            var handler = BuildHandler(tips, matches, users);

            var result = (await handler.Handle(
                new GetRanglisteQuery(),
                CancellationToken.None)).ToList();

            Assert.AreEqual(7, result[0].TotalPoints);
        }

        [TestMethod]
        public async Task TwoUsers_AreRankedCorrectly()
        {
            var home1 = Guid.NewGuid();
            var away1 = Guid.NewGuid();

            var home2 = Guid.NewGuid();
            var away2 = Guid.NewGuid();

            var tips = new WmTip[]
            {
                MakeGroupTip(_user1Id, _match1Id, 2, 1),
                MakeKoTip(_user1Id, _match2Id, home2),

                MakeGroupTip(_user2Id, _match1Id, 0, 0),
                MakeKoTip(_user2Id, _match2Id, away2)
            };

            var matches = new[]
            {
                MakeMatch(_match1Id, 2, 1, home1, away1),
                MakeMatch(_match2Id, 1, 0, home2, away2)
            };

            var users = new[]
            {
                MakeUser(_user1Id, "Alice"),
                MakeUser(_user2Id, "Bob")
            };

            var handler = BuildHandler(tips, matches, users);

            var result = (await handler.Handle(
                new GetRanglisteQuery(),
                CancellationToken.None)).ToList();

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