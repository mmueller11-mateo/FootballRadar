using FootballRadar.TippSpiel.Business.Queries;
using FootballRadar.TippSpiel.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.TippSpiel.WebApp.Controllers
{
    public class MeineTippsController : Controller
    {
        private readonly IMediator mediator;

        public MeineTippsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var tipperId = HttpContext.Session.GetString("TipperId");
            var tipperName = HttpContext.Session.GetString("TipperName");

            if (tipperId == null)
            {
                return RedirectToAction(nameof(EnterName));
            }

            var tips = await mediator.Send(new GetTipsByTipperQuery { TipperId = Guid.Parse(tipperId) });
            var model = new MeineTippsViewModel
            {
                TipperName = tipperName ?? string.Empty,
                Tips = new List<MeineTippViewModel>()
            };

            foreach (var tip in tips.OrderBy(t => t.MatchId))
            {
                var match = await mediator.Send(new GetMatchByIdQuery { MatchId = tip.MatchId });
                if (match == null)
                {
                    continue;
                }

                model.Tips.Add(new MeineTippViewModel
                {
                    HomeTeam = match.HomeTeam,
                    AwayTeam = match.AwayTeam,
                    KickoffUtc = match.KickoffUtc,
                    Phase = match.Phase,
                    PredictedHome = tip.PredictedHome,
                    PredictedAway = tip.PredictedAway,
                    Points = tip.Points,
                    HomeScore = match.HomeScore,
                    AwayScore = match.AwayScore
                });
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult EnterName()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EnterName(EnterNameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var tipper = await mediator.Send(new GetTipperByNameQuery { Name = model.Name });
            if (tipper == null)
            {
                ModelState.AddModelError(string.Empty, "Kein Tipper mit diesem Namen gefunden.");
                return View(model);
            }

            HttpContext.Session.SetString("TipperId", tipper.Id.ToString());
            HttpContext.Session.SetString("TipperName", tipper.Name);

            return RedirectToAction(nameof(Index));
        }
    }
}