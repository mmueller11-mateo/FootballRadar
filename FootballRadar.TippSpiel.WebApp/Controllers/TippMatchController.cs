using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.TippSpiel.Business.Commands;
using FootballRadar.TippSpiel.Business.Queries;
using FootballRadar.TippSpiel.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.TippSpiel.WebApp.Controllers
{
    public class TippMatchController : Controller
    {
        private readonly IMediator mediator;

        public TippMatchController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var matches = await mediator.Send(new GetMatchesQuery());
            var upcomingMatches = matches
                .Where(m => m.KickoffUtc > DateTime.UtcNow)
                .OrderBy(m => m.KickoffUtc)
                .ToList();

            var tipperId = HttpContext.Session.GetString("TipperId");
            var tipperName = HttpContext.Session.GetString("TipperName");

            var model = new TippMatchIndexViewModel
            {
                Matches = upcomingMatches,
                TipperName = tipperName,
                ExistingTips = new List<Tip>()
            };

            if (tipperId != null)
            {
                var tips = await mediator.Send(new GetTipsByTipperQuery { TipperId = Guid.Parse(tipperId) });
                model.ExistingTips = tips.ToList();
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult EnterName(Guid matchId)
        {
            var model = new EnterNameViewModel { MatchId = matchId };
            return View(model);
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
                tipper = await mediator.Send(new CreateTipperCommand { Name = model.Name });
            }

            HttpContext.Session.SetString("TipperId", tipper.Id.ToString());
            HttpContext.Session.SetString("TipperName", tipper.Name);

            return RedirectToAction(nameof(Tip), new { matchId = model.MatchId });
        }

        [HttpPost]
        public async Task<IActionResult> Tip(TipViewModel model)
        {
            var tipperId = HttpContext.Session.GetString("TipperId");
            if (tipperId == null)
            {
                return RedirectToAction(nameof(EnterName), new { matchId = model.MatchId });
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var matches = await mediator.Send(new GetMatchesQuery());
            var match = matches.FirstOrDefault(m => m.Id == model.MatchId);
            if (match == null || match.KickoffUtc <= DateTime.UtcNow)
            {
                return RedirectToAction(nameof(Index));
            }

            await mediator.Send(new SaveTipCommand
            {
                TipperId = Guid.Parse(tipperId),
                MatchId = model.MatchId,
                PredictedHome = model.PredictedHome,
                PredictedAway = model.PredictedAway
            });

            return RedirectToAction(nameof(Index));
        }
    }
}