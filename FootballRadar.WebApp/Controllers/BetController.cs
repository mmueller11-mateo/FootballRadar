using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Entities.Betting.Enums;
using FootballRadar.Business.Services.Commands;
using FootballRadar.Business.Services.Queries;
using FootballRadar.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FootballRadar.WebApp.Controllers
{
    [Authorize]
    public class BetController : Controller
    {
        private readonly IMediator mediator;
        private readonly IWalletRepository walletRepository;
        private readonly ITeamRepository teamRepository;
        private readonly IBetRepository betRepository;
        private readonly IPredictionMarketRepository predictionMarketRepository;
        private readonly IMatchRepository matchRepository;

        public BetController(IMediator mediator, IWalletRepository walletRepository, ITeamRepository teamRepository, IBetRepository betRepository, IPredictionMarketRepository predictionMarketRepository, IMatchRepository matchRepository)
        {
            this.mediator = mediator;
            this.walletRepository = walletRepository;
            this.teamRepository = teamRepository;
            this.betRepository = betRepository;
            this.predictionMarketRepository = predictionMarketRepository;
            this.matchRepository = matchRepository;
        }

        [HttpGet]
        public async Task<IActionResult> PlaceBet(Guid matchId, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var match = await mediator.Send(new GetMatchByIdQuery { MatchId = matchId });
            if (match == null)
                return NotFound();

            var wallet = await walletRepository.GetByUserIdAsync(userId);
            var homeTeam = await teamRepository.GetByIdAsync(match.HomeTeamId);
            var awayTeam = await teamRepository.GetByIdAsync(match.AwayTeamId);
            var h2hMatches = await mediator.Send(new GetHeadToHeadQuery
            {
                HomeTeamId = match.HomeTeamId,
                AwayTeamId = match.AwayTeamId
            });
            var allTeams = await teamRepository.GetAllAsync(cancellationToken);
            var h2h = h2hMatches.Select(m => new HeadToHeadViewModel
            {
                Date = m.Date,
                HomeTeam = allTeams.FirstOrDefault(t => t.Id == m.HomeTeamId)?.Name ?? "Unknown",
                AwayTeam = allTeams.FirstOrDefault(t => t.Id == m.AwayTeamId)?.Name ?? "Unknown",
                HomeGoals = m.HomeGoals,
                AwayGoals = m.AwayGoals
            });

            var vm = new PlaceBetViewModel
            {
                MatchId = matchId,
                HomeTeam = homeTeam?.Name ?? "Unknown",
                AwayTeam = awayTeam?.Name ?? "Unknown",
                HomeLogo = homeTeam?.Logo,
                AwayLogo = awayTeam?.Logo,
                MatchDate = match.Date,
                Round = match.Round,
                AvailableCredits = wallet?.Credits ?? 0,
                HeadToHead = h2h
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceBet(PlaceBetViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var status = await mediator.Send(new PlaceMatchBetCommand
            {
                UserId = userId,
                MatchId = vm.MatchId,
                Credits = vm.Credits,
                Prediction = vm.Prediction
            });

            if (status.Code == BetStatusCode.Rejected)
            {
                ModelState.AddModelError("", status.ErrorMessage ?? "Bet rejected.");
                return View(vm);
            }
            return RedirectToAction(nameof(BetConfirmed));
        }

        [HttpGet]
        public async Task<IActionResult> MyBets(CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var bets = await mediator.Send(new GetUserBetsQuery { UserId = userId });
            var allTeams = await teamRepository.GetAllAsync(cancellationToken);

            var items = new List<UserBetItemViewModel>();
            foreach (var bet in bets)
            {
                var market = await predictionMarketRepository.GetByIdAsync(bet.PredictionMarketId);
                if (market is not MatchPredictionMarket matchMarket) continue;

                var match = await matchRepository.GetByIdAsync(matchMarket.MatchId);
                if (match == null) continue;

                var homeTeam = allTeams.FirstOrDefault(t => t.Id == match.HomeTeamId);
                var awayTeam = allTeams.FirstOrDefault(t => t.Id == match.AwayTeamId);

                bool? isWon = null;
                decimal? payout = null;
                if (matchMarket.IsSettled && match.HomeGoals.HasValue && match.AwayGoals.HasValue)
                {
                    MatchPrediction correctPrediction;
                    if (match.HomeGoals > match.AwayGoals)
                        correctPrediction = MatchPrediction.HomeWin;
                    else if (match.AwayGoals > match.HomeGoals)
                        correctPrediction = MatchPrediction.AwayWin;
                    else
                        correctPrediction = MatchPrediction.Draw;

                    isWon = bet.Prediction == correctPrediction;
                    if (isWon == true)
                        payout = bet.Credits * (1 + matchMarket.Reward / 100m);
                }

                items.Add(new UserBetItemViewModel
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

            var settledBets = items.Where(i => i.IsSettled).ToList();
            var vm = new UserBetsViewModel
            {
                OpenBets = items.Where(i => !i.IsSettled).ToList(),
                SettledBets = settledBets,
                TotalBets = settledBets.Count,
                WonBets = settledBets.Count(b => b.IsWon == true),
                LostBets = settledBets.Count(b => b.IsWon == false),
                TotalWinnings = settledBets.Where(b => b.IsWon == true).Sum(b => b.Payout ?? 0),
                TotalStaked = settledBets.Sum(b => b.Credits)
            };
            return View(vm);
        }

        [HttpGet]
        public IActionResult BetConfirmed()
        {
            return View();
        }
    }
}