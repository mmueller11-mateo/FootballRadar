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

            // Exact result → 4 pts
            if (predictedHome == actualHome && predictedAway == actualAway)
                return 4;

            int predictedDiff = predictedHome - predictedAway;
            int actualDiff = actualHome - actualAway;

            // Wrong tendency → 0 pts (check first!)
            if (Math.Sign(predictedDiff) != Math.Sign(actualDiff))
                return 0;

            // Draws have no goal-difference tier → 2 pts
            if (actualDiff == 0)
                return 2;

            // Correct goal difference (wins only) → 3 pts
            if (predictedDiff == actualDiff)
                return 3;

            // Correct tendency only → 2 pts
            return 2;
        }
    }
}