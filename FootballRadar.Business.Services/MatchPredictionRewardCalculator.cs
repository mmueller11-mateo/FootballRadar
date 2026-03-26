using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.LeagueEntities;

namespace FootballRadar.Business.Services
{
    internal sealed class MatchPredictionRewardCalculator : IMatchPredictionRewardCalculator
    {
        private readonly ILeagueRepository _leagueRepository;

        public MatchPredictionRewardCalculator(ILeagueRepository leagueRepository)
        {
            this._leagueRepository = leagueRepository;
        }

        public async Task<int> CalculateReward(Match match)
        {
            var topLeagues = await _leagueRepository.GetTopLeaguesAsync();
            if (topLeagues.Any(l => l.Id == match.LeagueId))
            {
                return 5;
            }
            else
            {
                return 3;
            }
        }
    }
}
