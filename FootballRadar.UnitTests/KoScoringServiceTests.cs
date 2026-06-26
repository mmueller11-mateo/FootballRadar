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
            var tip = new WmTip { HomeGoals = 2, AwayGoals = 1 };
            var match = new Match { HomeGoals = null, AwayGoals = null };

            Assert.AreEqual(0, KoScoringService.Calculate(tip, match));
        }

        [TestMethod]
        public void Calculate_Returns_4_When_Exact_Result()
        {
            var tip = new WmTip { HomeGoals = 2, AwayGoals = 1 };
            var match = new Match { HomeGoals = 2, AwayGoals = 1 };

            Assert.AreEqual(4, KoScoringService.Calculate(tip, match));
        }

        [TestMethod]
        public void Calculate_Returns_3_When_Correct_Difference()
        {
            var tip = new WmTip { HomeGoals = 3, AwayGoals = 1 };
            var match = new Match { HomeGoals = 2, AwayGoals = 0 };

            Assert.AreEqual(3, KoScoringService.Calculate(tip, match));
        }

        [TestMethod]
        public void Calculate_Returns_2_When_Correct_Tendency()
        {
            var tip = new WmTip { HomeGoals = 3, AwayGoals = 1 };
            var match = new Match { HomeGoals = 1, AwayGoals = 0 };

            Assert.AreEqual(2, KoScoringService.Calculate(tip, match));
        }

        [TestMethod]
        public void Calculate_Returns_0_When_Wrong_Tendency()
        {
            var tip = new WmTip { HomeGoals = 2, AwayGoals = 1 };
            var match = new Match { HomeGoals = 0, AwayGoals = 3 };

            Assert.AreEqual(0, KoScoringService.Calculate(tip, match));
        }
    }
}