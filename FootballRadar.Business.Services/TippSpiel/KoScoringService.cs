using FootballRadar.Business.Entities.LeagueEntities;

namespace FootballRadar.Business.Services.TippSpiel
{
    public static class KoScoringService
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

            if (Math.Sign(actualDiff) != Math.Sign(predictedDiff))
                return 0;

            if (actualHome == predictedHome && actualAway == predictedAway)
                return 4;

            if (actualDiff == predictedDiff)
                return 3;

            return 2;
        }
    }
}