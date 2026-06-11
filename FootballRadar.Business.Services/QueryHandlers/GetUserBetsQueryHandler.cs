using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Entities.Betting.Enums;
using FootballRadar.Business.Services.Queries;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    internal class GetUserBetsQueryHandler : IRequestHandler<GetUserBetsQuery, UserBetsResult>
    {
        private readonly IBetRepository betRepository;
        private readonly ITeamRepository teamRepository;
        private readonly IPredictionMarketRepository predictionMarketRepository;
        private readonly IMatchRepository matchRepository;

        public GetUserBetsQueryHandler(IBetRepository betRepository, ITeamRepository teamRepository, IPredictionMarketRepository predictionMarketRepository, IMatchRepository matchRepository)
        {
            this.betRepository = betRepository;
            this.teamRepository = teamRepository;
            this.predictionMarketRepository = predictionMarketRepository;
            this.matchRepository = matchRepository;
        }

        public async Task<UserBetsResult> Handle(GetUserBetsQuery request, CancellationToken cancellationToken)
        {
            var bets = await betRepository.GetMatchBetsByUserIdAsync(request.UserId, cancellationToken);
            var allTeams = await teamRepository.GetAllAsync(cancellationToken);
            var items = new List<UserBetItemResult>();

            foreach (var bet in bets.OfType<WinnerBet>())
            {
                var market = await predictionMarketRepository.GetByIdAsync(bet.PredictionMarketId, cancellationToken);
                if (market is not MatchPredictionMarket matchMarket) continue;

                var match = await matchRepository.GetByIdAsync(matchMarket.MatchId, cancellationToken);
                if (match == null) continue;

                var homeTeam = allTeams.FirstOrDefault(t => t.Id == match.HomeTeamId);
                var awayTeam = allTeams.FirstOrDefault(t => t.Id == match.AwayTeamId);

                bool? isWon = null;
                decimal? payout = null;
                if (matchMarket.IsSettled && match.HomeGoals.HasValue && match.AwayGoals.HasValue)
                {
                    var correctPrediction = match.HomeGoals > match.AwayGoals ? MatchPrediction.HomeWin
                        : match.AwayGoals > match.HomeGoals ? MatchPrediction.AwayWin
                        : MatchPrediction.Draw;

                    isWon = bet.Prediction == correctPrediction;
                    if (isWon == true)
                        payout = bet.Credits * (1 + matchMarket.Reward / 100m);
                }

                items.Add(new UserBetItemResult
                {
                    BetId = bet.Id,
                    HomeTeam = homeTeam?.Name ?? "Unknown",
                    AwayTeam = awayTeam?.Name ?? "Unknown",
                    HomeLogo = homeTeam?.Logo,
                    AwayLogo = awayTeam?.Logo,
                    MatchDate = match.Date,
                    Prediction = bet.Prediction,
                    Credits = bet.Credits,
                    PlacedAt = bet.PlacedAt,
                    IsSettled = matchMarket.IsSettled,
                    IsWon = isWon,
                    HomeGoals = match.HomeGoals,
                    AwayGoals = match.AwayGoals,
                    Payout = payout,
                    Reward = matchMarket.Reward
                });
            }

            return new UserBetsResult { Items = items };
        }
    }
}