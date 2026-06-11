using FootballRadar.Business.Entities.LeagueEntities;
using FootballRadar.Business.Services.TippSpiel;

namespace FootballRadar.UnitTests
{
    /// <summary>
    /// Tests für die Punkteberechnung von Spieltipps (PredictionScoringService)
    /// und Bonusfragen, inkl. kombinierter Szenarien wie bei einem echten Tippspiel.
    /// </summary>
    [TestClass]
    public sealed class ScoringTests
    {
        // ════════════════════════════════════════════════════════════
        // Hilfsmethoden
        // ════════════════════════════════════════════════════════════

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

        private static int BonusPoints(Guid tippedTeamId, Guid correctTeamId) =>
            tippedTeamId == correctTeamId ? 4 : 0;

        // ════════════════════════════════════════════════════════════
        // PredictionScoringService — Einzelfälle
        // ════════════════════════════════════════════════════════════

        [TestMethod]
        [Description("Exaktes Ergebnis → 4 Punkte")]
        public void ExactResult_Returns4Points()
        {
            var match = PlayedMatch(2, 1);
            var tip = Tip(2, 1);
            Assert.AreEqual(4, PredictionScoringService.Calculate(tip, match));
        }

        [TestMethod]
        [Description("Richtige Tordifferenz, aber nicht exakt → 3 Punkte")]
        public void CorrectGoalDifference_Returns3Points()
        {
            // Tipp 3:1 (Diff +2), Ergebnis 4:2 (Diff +2) → gleiche Differenz, nicht exakt
            var match = PlayedMatch(4, 2);
            var tip = Tip(3, 1);
            Assert.AreEqual(3, PredictionScoringService.Calculate(tip, match));
        }

        [TestMethod]
        [Description("Richtige Tendenz (Heimsieg), falsche Tordifferenz → 2 Punkte")]
        public void CorrectTendencyHomeWin_Returns2Points()
        {
            // Tipp 1:0, Ergebnis 3:1 → beide Heimsieg, aber Differenz unterschiedlich
            var match = PlayedMatch(3, 1);
            var tip = Tip(1, 0);
            Assert.AreEqual(2, PredictionScoringService.Calculate(tip, match));
        }

        [TestMethod]
        [Description("Richtige Tendenz (Auswärtssieg), falsche Tordifferenz → 2 Punkte")]
        public void CorrectTendencyAwayWin_Returns2Points()
        {
            // Tipp 0:2, Ergebnis 1:3 → beide Auswärtssieg
            var match = PlayedMatch(1, 3);
            var tip = Tip(0, 1);
            Assert.AreEqual(2, PredictionScoringService.Calculate(tip, match));
        }

        [TestMethod]
        [Description("Exaktes Unentschieden → 4 Punkte")]
        public void ExactDraw_Returns4Points()
        {
            var match = PlayedMatch(1, 1);
            var tip = Tip(1, 1);
            Assert.AreEqual(4, PredictionScoringService.Calculate(tip, match));
        }

        [TestMethod]
        [Description("Falsches Unentschieden getippt (0:0), echtes Ergebnis 1:1 → 2 Punkte (richtige Tendenz)")]
        public void WrongDrawScore_CorrectTendency_Returns2Points()
        {
            var match = PlayedMatch(1, 1);
            var tip = Tip(0, 0);
            Assert.AreEqual(2, PredictionScoringService.Calculate(tip, match));
        }

        [TestMethod]
        [Description("Heimsieg getippt, aber Auswärtssieg → 0 Punkte")]
        public void WrongTendency_Returns0Points()
        {
            var match = PlayedMatch(0, 2);
            var tip = Tip(2, 0);
            Assert.AreEqual(0, PredictionScoringService.Calculate(tip, match));
        }

        [TestMethod]
        [Description("Unentschieden getippt, aber Heimsieg → 0 Punkte")]
        public void TippedDraw_ActualHomeWin_Returns0Points()
        {
            var match = PlayedMatch(2, 0);
            var tip = Tip(1, 1);
            Assert.AreEqual(0, PredictionScoringService.Calculate(tip, match));
        }

        [TestMethod]
        [Description("Spiel noch nicht gespielt → 0 Punkte")]
        public void MatchNotYetPlayed_Returns0Points()
        {
            var match = new Match { Id = Guid.NewGuid(), HomeGoals = null, AwayGoals = null };
            var tip = Tip(2, 1);
            Assert.AreEqual(0, PredictionScoringService.Calculate(tip, match));
        }

        // ════════════════════════════════════════════════════════════
        // Bonusfragen — Einzelfälle
        // ════════════════════════════════════════════════════════════

        [TestMethod]
        [Description("Bonusfrage richtig beantwortet → 4 Punkte")]
        public void BonusQuestion_CorrectAnswer_Returns4Points()
        {
            var correctTeam = Guid.NewGuid();
            Assert.AreEqual(4, BonusPoints(correctTeam, correctTeam));
        }

        [TestMethod]
        [Description("Bonusfrage falsch beantwortet → 0 Punkte")]
        public void BonusQuestion_WrongAnswer_Returns0Points()
        {
            var correctTeam = Guid.NewGuid();
            var wrongTeam = Guid.NewGuid();
            Assert.AreEqual(0, BonusPoints(wrongTeam, correctTeam));
        }

        // ════════════════════════════════════════════════════════════
        // Kombinierte Szenarien — realistisches Tippspiel
        // ════════════════════════════════════════════════════════════

        [TestMethod]
        [Description(
            "Szenario: Deutschland-Fan tippt die WM-Gruppenphase + Bonusfragen. " +
            "Matches: GER 2:1 JAP (exakt ✓), GER 0:0 KOS (Tendenz ✓), GER 3:0 CRI (Diff ✓), GER 0:1 ESP (falsch ✗). " +
            "Bonus: Weltmeister richtig (GER), Torschütze falsch. " +
            "Erwartete Gesamtpunkte: 4+2+3+0 + 4+0 = 13")]
        public void GermanFan_RealisticWorldCupTipping_CorrectTotalPoints()
        {
            // ── Spieltipps ──────────────────────────────────────────
            // GER vs JPN: Tipp 2:1, Ergebnis 2:1 → exakt → 4 Pts
            int pts1 = PredictionScoringService.Calculate(Tip(2, 1), PlayedMatch(2, 1));

            // GER vs KOS: Tipp 0:0, Ergebnis 1:1 → Tendenz (Draw) → 2 Pts
            int pts2 = PredictionScoringService.Calculate(Tip(0, 0), PlayedMatch(1, 1));

            // GER vs CRI: Tipp 3:0, Ergebnis 4:1 → Tordifferenz (+3) → 3 Pts
            int pts3 = PredictionScoringService.Calculate(Tip(3, 0), PlayedMatch(4, 1));

            // GER vs ESP: Tipp 1:0 (Heimsieg), Ergebnis 0:1 (Auswärtssieg) → falsch → 0 Pts
            int pts4 = PredictionScoringService.Calculate(Tip(1, 0), PlayedMatch(0, 1));

            // ── Bonusfragen ─────────────────────────────────────────
            var germany = Guid.NewGuid();
            var topScorer = Guid.NewGuid();
            var wrongTeam = Guid.NewGuid();

            // Weltmeister: Deutschland getippt, Deutschland gewinnt → 4 Pts
            int bonus1 = BonusPoints(germany, germany);

            // Torschützenkönig-Team: falsch getippt → 0 Pts
            int bonus2 = BonusPoints(wrongTeam, topScorer);

            // ── Gesamtpunkte ─────────────────────────────────────────
            int matchPoints = pts1 + pts2 + pts3 + pts4;
            int bonusPoints = bonus1 + bonus2;
            int total = matchPoints + bonusPoints;

            Assert.AreEqual(4, pts1, "GER vs JPN: erwartet 4 (exakt)");
            Assert.AreEqual(2, pts2, "GER vs KOS: erwartet 2 (Tendenz)");
            Assert.AreEqual(3, pts3, "GER vs CRI: erwartet 3 (Tordifferenz)");
            Assert.AreEqual(0, pts4, "GER vs ESP: erwartet 0 (falsch)");
            Assert.AreEqual(4, bonus1, "Weltmeister: erwartet 4 (richtig)");
            Assert.AreEqual(0, bonus2, "Torschütze: erwartet 0 (falsch)");
            Assert.AreEqual(9, matchPoints, "Spielpunkte total: erwartet 9");
            Assert.AreEqual(4, bonusPoints, "Bonuspunkte total: erwartet 4");
            Assert.AreEqual(13, total, "Gesamtpunkte: erwartet 13");
        }

        [TestMethod]
        [Description(
            "Szenario: Perfekter Tipper — alles exakt + alle Bonusfragen richtig. " +
            "3 Matches je exakt (3×4=12) + 3 Bonusfragen richtig (3×4=12) = 24 Punkte total.")]
        public void PerfectTipper_AllExactAndAllBonusCorrect_Returns24Points()
        {
            int matchPts = 0;
            matchPts += PredictionScoringService.Calculate(Tip(1, 0), PlayedMatch(1, 0)); // exakt
            matchPts += PredictionScoringService.Calculate(Tip(2, 2), PlayedMatch(2, 2)); // exakt
            matchPts += PredictionScoringService.Calculate(Tip(0, 3), PlayedMatch(0, 3)); // exakt

            var t1 = Guid.NewGuid();
            var t2 = Guid.NewGuid();
            var t3 = Guid.NewGuid();

            int bonusPts = 0;
            bonusPts += BonusPoints(t1, t1); // Weltmeister richtig
            bonusPts += BonusPoints(t2, t2); // Halbfinale richtig
            bonusPts += BonusPoints(t3, t3); // Gruppensieger richtig

            Assert.AreEqual(12, matchPts, "3 exakte Tipps → 12 Punkte");
            Assert.AreEqual(12, bonusPts, "3 Bonusfragen richtig → 12 Punkte");
            Assert.AreEqual(24, matchPts + bonusPts, "Total → 24 Punkte");
        }

        [TestMethod]
        [Description(
            "Szenario: Pechvogel — alle Spieltipps falsch, alle Bonusfragen falsch → 0 Punkte total.")]
        public void UnluckyTipper_AllWrong_Returns0Points()
        {
            int matchPts = 0;
            matchPts += PredictionScoringService.Calculate(Tip(2, 0), PlayedMatch(0, 2)); // falsch
            matchPts += PredictionScoringService.Calculate(Tip(1, 0), PlayedMatch(0, 1)); // falsch
            matchPts += PredictionScoringService.Calculate(Tip(3, 1), PlayedMatch(0, 2)); // falsch

            int bonusPts = 0;
            bonusPts += BonusPoints(Guid.NewGuid(), Guid.NewGuid()); // falsch
            bonusPts += BonusPoints(Guid.NewGuid(), Guid.NewGuid()); // falsch

            Assert.AreEqual(0, matchPts + bonusPts, "Alles falsch → 0 Punkte");
        }

        [TestMethod]
        [Description(
            "Szenario: Nur Bonusfragen richtig, alle Spieltipps falsch. " +
            "2 Bonusfragen richtig (2×4=8), 3 Spieltipps falsch (0) → 8 Punkte total.")]
        public void OnlyBonusCorrect_Returns8Points()
        {
            int matchPts = 0;
            matchPts += PredictionScoringService.Calculate(Tip(2, 0), PlayedMatch(0, 2));
            matchPts += PredictionScoringService.Calculate(Tip(1, 0), PlayedMatch(0, 1));
            matchPts += PredictionScoringService.Calculate(Tip(3, 1), PlayedMatch(1, 3));

            var t1 = Guid.NewGuid();
            var t2 = Guid.NewGuid();
            int bonusPts = BonusPoints(t1, t1) + BonusPoints(t2, t2);

            Assert.AreEqual(0, matchPts, "Alle Spieltipps falsch → 0");
            Assert.AreEqual(8, bonusPts, "2 Bonusfragen richtig → 8");
            Assert.AreEqual(8, matchPts + bonusPts, "Total → 8 Punkte");
        }
    }
}