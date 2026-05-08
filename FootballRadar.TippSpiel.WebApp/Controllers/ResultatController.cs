using FootballRadar.TippSpiel.Abstractions;
using FootballRadar.TippSpiel.Business.Queries;
using FootballRadar.TippSpiel.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.TippSpiel.WebApp.Controllers
{
    public class ResultatController : Controller
    {
        private readonly IMediator mediator;
        private readonly ITipperRepository tipperRepository;

        public ResultatController(IMediator mediator, ITipperRepository tipperRepository)
        {
            this.mediator = mediator;
            this.tipperRepository = tipperRepository;
        }

        public async Task<IActionResult> Index()
        {
            var playedMatches = await mediator.Send(new GetPlayedMatchesQuery());
            var model = new ResultatIndexViewModel
            {
                Matches = new List<ResultatMatchViewModel>()
            };

            foreach (var match in playedMatches.OrderByDescending(m => m.KickoffUtc))
            {
                var tips = await mediator.Send(new GetTipsByMatchQuery { MatchId = match.Id });
                var tipViewModels = new List<ResultatTipViewModel>();

                foreach (var tip in tips)
                {
                    var tipper = await tipperRepository.GetByIdAsync(tip.TipperId);
                    tipViewModels.Add(new ResultatTipViewModel
                    {
                        TipperName = tipper?.Name ?? "Unbekannt",
                        PredictedHome = tip.PredictedHome,
                        PredictedAway = tip.PredictedAway,
                        Points = tip.Points ?? 0
                    });
                }

                model.Matches.Add(new ResultatMatchViewModel
                {
                    HomeTeam = match.HomeTeam,
                    AwayTeam = match.AwayTeam,
                    HomeScore = match.HomeScore!.Value,
                    AwayScore = match.AwayScore!.Value,
                    KickoffUtc = match.KickoffUtc,
                    Phase = match.Phase,
                    Tips = tipViewModels.OrderByDescending(t => t.Points).ToList()
                });
            }

            return View(model);
        }
    }
}