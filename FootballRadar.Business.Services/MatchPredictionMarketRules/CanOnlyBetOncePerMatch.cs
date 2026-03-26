using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.LeagueEntities;

namespace FootballRadar.Business.Services.MatchPredictionMarketRules
{
    sealed class CanOnlyBetOncePerMatch : MatchPredictionMarketRule
    {
        private readonly Guid _userId;
        private readonly IBetRepository _betRepository;

        public CanOnlyBetOncePerMatch(Match match, Guid userId, IBetRepository betRepository) : base(match)
        {
            _userId = userId;
            _betRepository = betRepository;
        }

        public override async Task<bool> Evaluate()
        {
            return !await _betRepository.HasUserBetOnMatchAsync(_userId, this.Match.Id);
        }

        public override string ErrorMessage { get; } = "You can only place one bet per match";
    }
}