using FootballRadar.Business.Entities.LeagueEntities;

namespace FootballRadar.Business.Services.TippSpiel
{
    public static class PredictionScoringService
    {
        public static int Calculate(WmTip tip, Match match)
        {
            if (match.HomeGoals is null || match.AwayGoals is null)
                return 0;

            int actualHome = match.HomeGoals.Value;
            int actualAway = match.AwayGoals.Value;
            int predictedHome = tip.HomeGoals;
            int predictedAway = tip.AwayGoals;

            int actualDiff = actualHome - actualAway;
            int predictedDiff = predictedHome - predictedAway;

            // Falsche Tendenz → 0
            if (Math.Sign(predictedDiff) != Math.Sign(actualDiff))
                return 0;

            // Exaktes Ergebnis
            if (predictedHome == actualHome && predictedAway == actualAway)
            {
                if (actualDiff == 0) return 4; // Unentschieden exakt
                if (actualDiff > 0) return 4; // Heimsieg exakt
                return 5;                       // Auswärtssieg exakt
            }

            // Unentschieden: kein Tordifferenz-Tier → direkt Tendenz
            if (actualDiff == 0)
                return 3;

            // Richtige Tordifferenz (nur bei Siegen)
            if (predictedDiff == actualDiff)
            {
                if (actualDiff > 0) return 3; // Heimsieg Tordifferenz
                return 4;                      // Auswärtssieg Tordifferenz
            }

            // Nur Tendenz
            if (actualDiff > 0) return 2; // Heimsieg Tendenz
            return 3;                      // Auswärtssieg Tendenz
        }
    }
}