using FootballRadar.Abstractions;
using FootballRadar.Business.Services.Queries;
using FootballRadar.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.WebApp.Controllers
{
    [Authorize]
    public class ResultatController : Controller
    {
        private readonly IMediator mediator;
        private readonly IUserRepository userRepository;
        private readonly INationalTeamRepository nationalTeamRepository;

        public ResultatController(IMediator mediator, IUserRepository userRepository, INationalTeamRepository nationalTeamRepository)
        {
            this.mediator = mediator;
            this.userRepository = userRepository;
            this.nationalTeamRepository = nationalTeamRepository;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var matches = await mediator.Send(new GetPlayedMatchesQuery(), cancellationToken);
            var teams = await nationalTeamRepository.GetAllAsync(cancellationToken);

            var model = new ResultatIndexViewModel
            {
                Matches = new List<ResultatMatchViewModel>()
            };

            foreach (var match in matches.OrderByDescending(m => m.Date))
            {
                var tips = await mediator.Send(new GetTipsByMatchQuery { MatchId = match.Id }, cancellationToken);
                var tipViewModels = new List<ResultatTipViewModel>();

                foreach (var tip in tips)
                {
                    var user = await userRepository.GetByIdAsync(tip.UserId, cancellationToken);
                    tipViewModels.Add(new ResultatTipViewModel
                    {
                        TipperName = user?.Name ?? "Unbekannt",
                        PredictedHome = tip.HomeGoals,
                        PredictedAway = tip.AwayGoals,
                        Points = tip.Points ?? 0
                    });
                }

                var home = teams.FirstOrDefault(t => t.Id == match.HomeNationalTeamId);
                var away = teams.FirstOrDefault(t => t.Id == match.AwayNationalTeamId);

                model.Matches.Add(new ResultatMatchViewModel
                {
                    HomeTeam = home?.Name ?? "Unknown",
                    AwayTeam = away?.Name ?? "Unknown",
                    HomeScore = match.HomeGoals!.Value,
                    AwayScore = match.AwayGoals!.Value,
                    KickoffUtc = match.Date,
                    WmGroup = match.WmGroup,
                    WmPhase = match.WmPhase!.Value,
                    Tips = tipViewModels.OrderByDescending(t => t.Points).ToList()
                });
            }

            return View(model);
        }
    }
}