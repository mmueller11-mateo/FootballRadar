using FootballRadar.Business.Entities.LeagueEntities;

namespace FootballRadar.Business.Services.TippSpiel
{
    public static class KoScoringService
    {
        public static int Calculate(WmTip tip, Match match)
        {
            if (!match.HomeGoals.HasValue || !match.AwayGoals.HasValue)
                return 0;

            var actualWinner =
                match.HomeGoals > match.AwayGoals
                    ? match.HomeNationalTeamId
                    : match.AwayNationalTeamId;

            if (tip.PredictedWinnerId == actualWinner)
                return 3; // oder 5, je nachdem wie wichtig KO sein soll

            return 0;
        }
    }
}
