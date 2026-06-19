using FootballRadar.Business.Entities.LeagueEntities;
using FootballRadar.Business.Services.TippSpiel;

namespace FootballRadar.UnitTests
{
    [TestClass]
    public sealed class ScoringTests
    {
        private static Match PlayedMatch(int homeGoals, int awayGoals) => new()
        {
            Id = Guid.NewGuid(),
            Date = DateTimeOffset.UtcNow.AddHours(-2),
            HomeGoals = homeGoals,
            AwayGoals = awayGoals
        };

        private static WmTip Tip(int homeGoals, int awayGoals) => new()
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            WmMatchId = Guid.NewGuid(),
            HomeGoals = homeGoals,
            AwayGoals = awayGoals
        };

        private static int BonusPoints(Guid tippedTeamId, Guid correctTeamId) => tippedTeamId == correctTeamId ? 4 : 0;

        [TestMethod]
        [Description("Ergebnis 2:1, Tipp 1:2 → 0 Punkte (falsche Tendenz)")]
        public void WrongTendency_HomeVsAway_Returns0()
        {
            Assert.AreEqual(0, PredictionScoringService.Calculate(Tip(1, 2), PlayedMatch(2, 1)));
        }

        [TestMethod]
        [Description("Heimsieg getippt, Auswärtssieg eingetreten → 0 Punkte")]
        public void HomeWin_Tipped_ActualAwayWin_Returns0()
        {
            Assert.AreEqual(0, PredictionScoringService.Calculate(Tip(2, 0), PlayedMatch(0, 2)));
        }

        [TestMethod]
        [Description("Unentschieden getippt, aber Heimsieg → 0 Punkte")]
        public void Draw_Tipped_ActualHomeWin_Returns0()
        {
            Assert.AreEqual(0, PredictionScoringService.Calculate(Tip(1, 1), PlayedMatch(2, 0)));
        }

        [TestMethod]
        [Description("Heimsieg Tendenz → 2 Punkte")]
        public void HomeWin_Tendency_Returns2()
        {
            // Tipp 1:0 (Diff +1), Ergebnis 3:1 (Diff +2) → beide Heimsieg, Differenz unterschiedlich
            Assert.AreEqual(2, PredictionScoringService.Calculate(Tip(1, 0), PlayedMatch(3, 1)));
        }

        [TestMethod]
        [Description("Heimsieg Tordifferenz → 3 Punkte")]
        public void HomeWin_CorrectDiff_Returns3()
        {
            // Tipp 1:0 (Diff +1), Ergebnis 2:1 (Diff +1)
            Assert.AreEqual(3, PredictionScoringService.Calculate(Tip(1, 0), PlayedMatch(2, 1)));
        }

        [TestMethod]
        [Description("Heimsieg Exakt → 4 Punkte")]
        public void HomeWin_Exact_Returns4()
        {
            Assert.AreEqual(4, PredictionScoringService.Calculate(Tip(2, 1), PlayedMatch(2, 1)));
        }

        [TestMethod]
        [Description("Auswärtssieg Tendenz → 2 Punkte")]
        public void AwayWin_Tendency_Returns2()
        {
            // Tipp 0:1 (Diff -1), Ergebnis 1:3 (Diff -2)
            Assert.AreEqual(2, PredictionScoringService.Calculate(Tip(0, 1), PlayedMatch(1, 3)));
        }

        [TestMethod]
        [Description("Auswärtssieg Tordifferenz → 3 Punkte")]
        public void AwayWin_CorrectDiff_Returns3()
        {
            // Tipp 0:2 (Diff -2), Ergebnis 1:3 (Diff -2)
            Assert.AreEqual(3, PredictionScoringService.Calculate(Tip(0, 2), PlayedMatch(1, 3)));
        }

        [TestMethod]
        [Description("Auswärtssieg Exakt → 4 Punkte")]
        public void AwayWin_Exact_Returns4()
        {
            Assert.AreEqual(4, PredictionScoringService.Calculate(Tip(0, 2), PlayedMatch(0, 2)));
        }

        [TestMethod]
        [Description("Unentschieden Tendenz (falsches Resultat) → 2 Punkte (kein Diff-Tier)")]
        public void Draw_Tendency_Returns2()
        {
            // Tipp 0:0, Ergebnis 1:1 → beide Unentschieden, aber nicht exakt
            Assert.AreEqual(2, PredictionScoringService.Calculate(Tip(0, 0), PlayedMatch(1, 1)));
        }

        [TestMethod]
        [Description("Unentschieden Tendenz (anderes Resultat) → 2 Punkte")]
        public void Draw_Tendency_DifferentScore_Returns2()
        {
            // Tipp 2:2, Ergebnis 1:1
            Assert.AreEqual(2, PredictionScoringService.Calculate(Tip(2, 2), PlayedMatch(1, 1)));
        }

        [TestMethod]
        [Description("Unentschieden Exakt → 4 Punkte")]
        public void Draw_Exact_Returns4()
        {
            Assert.AreEqual(4, PredictionScoringService.Calculate(Tip(1, 1), PlayedMatch(1, 1)));
        }

        [TestMethod]
        [Description("Spiel noch nicht gespielt → 0 Punkte")]
        public void MatchNotYetPlayed_Returns0()
        {
            var match = new Match { Id = Guid.NewGuid(), HomeGoals = null, AwayGoals = null };
            Assert.AreEqual(0, PredictionScoringService.Calculate(Tip(2, 1), match));
        }

        [TestMethod]
        [Description("Bonusfrage richtig → 4 Punkte")]
        public void BonusQuestion_Correct_Returns4()
        {
            var team = Guid.NewGuid();
            Assert.AreEqual(4, BonusPoints(team, team));
        }

        [TestMethod]
        [Description("Bonusfrage falsch → 0 Punkte")]
        public void BonusQuestion_Wrong_Returns0()
        {
            Assert.AreEqual(0, BonusPoints(Guid.NewGuid(), Guid.NewGuid()));
        }

        [TestMethod]
        [Description(
            "Realistisches Szenario: " +
            "2:1 exakt (4) + 0:0 vs 1:1 Tendenz (2) + 3:0 vs 4:1 Diff (3) + 1:0 vs 0:1 falsch (0) " +
            "+ Weltmeister richtig (4) + Torschütze falsch (0) = 13 Punkte")]
        public void RealisticScenario_Returns13()
        {
            int pts1 = PredictionScoringService.Calculate(Tip(2, 1), PlayedMatch(2, 1)); // exakt Heimsieg → 4
            int pts2 = PredictionScoringService.Calculate(Tip(0, 0), PlayedMatch(1, 1)); // Tendenz Unentschieden → 2
            int pts3 = PredictionScoringService.Calculate(Tip(3, 0), PlayedMatch(4, 1)); // Diff Heimsieg → 3
            int pts4 = PredictionScoringService.Calculate(Tip(1, 0), PlayedMatch(0, 1)); // falsch → 0

            var winner = Guid.NewGuid();
            int bonus1 = BonusPoints(winner, winner);                 // Weltmeister richtig → 4
            int bonus2 = BonusPoints(Guid.NewGuid(), Guid.NewGuid()); // Torschütze falsch → 0

            Assert.AreEqual(4, pts1, "Exakt Heimsieg → 4");
            Assert.AreEqual(2, pts2, "Tendenz Unentschieden → 2");
            Assert.AreEqual(3, pts3, "Diff Heimsieg → 3");
            Assert.AreEqual(0, pts4, "Falsche Tendenz → 0");
            Assert.AreEqual(4, bonus1, "Weltmeister richtig → 4");
            Assert.AreEqual(0, bonus2, "Torschütze falsch → 0");
            Assert.AreEqual(9, pts1 + pts2 + pts3 + pts4, "Spielpunkte → 9");
            Assert.AreEqual(4, bonus1 + bonus2, "Bonuspunkte → 4");
            Assert.AreEqual(13, pts1 + pts2 + pts3 + pts4 + bonus1 + bonus2, "Total → 13");
        }

        [TestMethod]
        [Description("Perfekter Tipper: Heimsieg exakt (4) + Unentschieden exakt (4) + Auswärtssieg exakt (4) + 3 Bonus richtig (12) = 24 Punkte")]
        public void PerfectTipper_Returns24()
        {
            int matchPts = 0;
            matchPts += PredictionScoringService.Calculate(Tip(1, 0), PlayedMatch(1, 0)); // Heimsieg exakt → 4
            matchPts += PredictionScoringService.Calculate(Tip(2, 2), PlayedMatch(2, 2)); // Unentschieden exakt → 4
            matchPts += PredictionScoringService.Calculate(Tip(0, 3), PlayedMatch(0, 3)); // Auswärtssieg exakt → 4

            var t1 = Guid.NewGuid();
            var t2 = Guid.NewGuid();
            var t3 = Guid.NewGuid();
            int bonusPts = BonusPoints(t1, t1) + BonusPoints(t2, t2) + BonusPoints(t3, t3);

            Assert.AreEqual(12, matchPts, "Spielpunkte → 12");
            Assert.AreEqual(12, bonusPts, "Bonuspunkte → 12");
            Assert.AreEqual(24, matchPts + bonusPts, "Total → 24");
        }

        [TestMethod]
        [Description("Pechvogel: alles falsche Tendenz + alle Bonus falsch → 0 Punkte")]
        public void UnluckyTipper_Returns0()
        {
            int matchPts = 0;
            matchPts += PredictionScoringService.Calculate(Tip(2, 0), PlayedMatch(0, 2));
            matchPts += PredictionScoringService.Calculate(Tip(1, 0), PlayedMatch(0, 1));
            matchPts += PredictionScoringService.Calculate(Tip(3, 1), PlayedMatch(0, 2));

            int bonusPts = BonusPoints(Guid.NewGuid(), Guid.NewGuid())
                         + BonusPoints(Guid.NewGuid(), Guid.NewGuid());

            Assert.AreEqual(0, matchPts + bonusPts, "Total → 0");
        }

        [TestMethod]
        [Description("Nur Bonus richtig: 3 Spiele falsche Tendenz (0) + 2 Bonus richtig (8) = 8 Punkte")]
        public void OnlyBonusCorrect_Returns8()
        {
            int matchPts = 0;
            matchPts += PredictionScoringService.Calculate(Tip(2, 0), PlayedMatch(0, 2));
            matchPts += PredictionScoringService.Calculate(Tip(1, 0), PlayedMatch(0, 1));
            matchPts += PredictionScoringService.Calculate(Tip(3, 1), PlayedMatch(1, 3));

            var t1 = Guid.NewGuid();
            var t2 = Guid.NewGuid();
            int bonusPts = BonusPoints(t1, t1) + BonusPoints(t2, t2);

            Assert.AreEqual(0, matchPts, "Spielpunkte → 0");
            Assert.AreEqual(8, bonusPts, "Bonuspunkte → 8");
            Assert.AreEqual(8, matchPts + bonusPts, "Total → 8");
        }
    }
}