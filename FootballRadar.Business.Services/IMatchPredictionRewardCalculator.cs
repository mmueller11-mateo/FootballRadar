using FootballRadar.Business.Entities.LeagueEntities;

namespace FootballRadar.Business.Services
{
    internal interface IMatchPredictionRewardCalculator
    {
        Task<int> CalculateReward(Match match);
    }
}
