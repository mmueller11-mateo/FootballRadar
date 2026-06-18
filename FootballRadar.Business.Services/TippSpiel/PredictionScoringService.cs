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

            // Falsche Tendenz
            if (Math.Sign(actualDiff) != Math.Sign(predictedDiff))
                return 0;

            // Exaktes Ergebnis
            if (actualHome == predictedHome &&
                actualAway == predictedAway)
            {
                return 4;
            }

            // Unentschieden (nicht exakt)
            if (actualDiff == 0)
            {
                return 2;
            }

            // Sieg mit richtiger Tordifferenz
            if (actualDiff == predictedDiff)
            {
                return 3;
            }

            // Nur richtige Tendenz
            return 2;
        }
    }
}