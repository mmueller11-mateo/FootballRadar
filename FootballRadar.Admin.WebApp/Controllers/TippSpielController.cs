using FootballRadar.Admin.Business.Services.Commands.Create;
using FootballRadar.Admin.Business.Services.Queries;
using FootballRadar.Admin.WebApp.Models;
using FootballRadar.Business.Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.Admin.WebApp.Controllers
{
    public class TippSpielController : Controller
    {

        [HttpGet]
        public async Task<IActionResult> MatchList(CancellationToken cancellationToken = default)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();
            var matches = await mediator.Send(new GetWmMatchesQuery(), cancellationToken);
            var teams = await mediator.Send(new GetNationalTeamsQuery(), cancellationToken);

            var all = matches.OrderBy(m => m.Date).Select(m =>
            {
                var home = teams.FirstOrDefault(t => t.Id == m.HomeNationalTeamId);
                var away = teams.FirstOrDefault(t => t.Id == m.AwayNationalTeamId);

                return new WmMatchAdminViewModel
                {
                    Id = m.Id,
                    Date = m.Date,
                    Phase = m.WmPhase?.ToString() ?? "-",
                    WmGroup = m.WmGroup,
                    HomeTeam = home?.Name ?? m.HomeQualificationCode ?? "?",
                    AwayTeam = away?.Name ?? m.AwayQualificationCode ?? "?",
                    HomeGoals = m.HomeGoals ?? 0,
                    AwayGoals = m.AwayGoals ?? 0,
                    IsKnockout = m.WmPhase != WmPhase.Group,
                    HasResult = m.HomeGoals.HasValue && m.AwayGoals.HasValue
                };
            }).ToList();

            return View(new WmMatchListViewModel
            {
                GroupMatches = all.Where(m => !m.IsKnockout).ToList(),
                KnockoutMatches = all.Where(m => m.IsKnockout).ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> SetGroupResult(Guid matchId, int homeGoals, int awayGoals, CancellationToken cancellationToken = default)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();
            await mediator.Send(new SetGroupMatchResultCommand
            {
                MatchId = matchId,
                HomeGoals = homeGoals,
                AwayGoals = awayGoals
            }, cancellationToken);

            TempData["SuccessMessage"] = "Resultat gespeichert & Punkte berechnet.";
            return RedirectToAction(nameof(MatchList));
        }

        [HttpPost]
        public async Task<IActionResult> SetKnockoutWinner(Guid matchId, string winner, CancellationToken cancellationToken = default)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();
            await mediator.Send(new SetKnockoutMatchWinnerCommand
            {
                MatchId = matchId,
                Winner = winner
            }, cancellationToken);

            TempData["SuccessMessage"] = "Sieger gespeichert & Punkte berechnet.";
            return RedirectToAction(nameof(MatchList));
        }

        [HttpPost]
        public async Task<IActionResult> RecalculatePoints(CancellationToken cancellationToken = default)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();
            await mediator.Send(new RecalculateAllPointsCommand(), cancellationToken);

            TempData["SuccessMessage"] = "Alle Punkte neu berechnet.";
            return RedirectToAction(nameof(MatchList));
        }
    }
}