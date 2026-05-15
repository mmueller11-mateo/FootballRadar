using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.TippSpiel.Business.Commands;
using FootballRadar.TippSpiel.Business.Queries;
using FootballRadar.TippSpiel.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FootballRadar.TippSpiel.WebApp.Controllers
{
    [Authorize]
    public class TippMatchController : Controller
    {
        private readonly IMediator mediator;

        private Guid CurrentUserId =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private string CurrentUserName =>
            User.FindFirstValue(ClaimTypes.Name)!;

        public TippMatchController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var matches = await mediator.Send(new GetMatchesQuery());

            var groupMatches = matches
                .Where(m => m.Phase.StartsWith("Group"))
                .ToList();

            var groups = groupMatches
                .GroupBy(m => m.Phase)
                .OrderBy(g => g.Key)
                .Select(g => BuildGroupStanding(g.Key, g.ToList()))
                .ToList();

            var model = new TippMatchIndexViewModel
            {
                TipperName = CurrentUserName,
                Groups = groups
            };

            return View(model);
        }

        public async Task<IActionResult> Group(string groupName)
        {
            var matches = await mediator.Send(new GetMatchesQuery());
            var now = DateTime.UtcNow;

            var groupMatches = matches
                .Where(m => m.Phase == groupName)
                .OrderBy(m => m.KickoffUtc)
                .ToList();

            var tips = await mediator.Send(new GetTipsByTipperQuery { TipperId = CurrentUserId });

            var model = new GroupDetailViewModel
            {
                GroupName = groupName,
                TipperName = CurrentUserName,
                PastMatches = groupMatches.Where(m => m.KickoffUtc <= now).ToList(),
                UpcomingMatches = groupMatches.Where(m => m.KickoffUtc > now).ToList(),
                ExistingTips = tips.ToList()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Tip(Guid matchId)
        {
            var matches = await mediator.Send(new GetMatchesQuery());
            var match = matches.FirstOrDefault(m => m.Id == matchId);

            if (match == null || match.KickoffUtc <= DateTime.UtcNow)
                return RedirectToAction(nameof(Index));

            var tips = await mediator.Send(new GetTipsByTipperQuery { TipperId = CurrentUserId });
            var existing = tips.FirstOrDefault(t => t.MatchId == matchId);

            var model = new TipViewModel
            {
                MatchId = matchId,
                PredictedHome = existing?.PredictedHome ?? 0,
                PredictedAway = existing?.PredictedAway ?? 0
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Tip(TipViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var matches = await mediator.Send(new GetMatchesQuery());
            var match = matches.FirstOrDefault(m => m.Id == model.MatchId);

            if (match == null || match.KickoffUtc <= DateTime.UtcNow)
                return RedirectToAction(nameof(Index));

            await mediator.Send(new SaveTipCommand
            {
                TipperId = CurrentUserId,
                MatchId = model.MatchId,
                PredictedHome = model.PredictedHome,
                PredictedAway = model.PredictedAway
            });

            return RedirectToAction(nameof(Group), new { groupName = match.Phase });
        }

        private static GroupStandingViewModel BuildGroupStanding(string groupName, List<TippMatch> matches)
        {
            var teamStats = new Dictionary<string, TeamStandingRow>();

            void EnsureTeam(string name)
            {
                if (!teamStats.ContainsKey(name))
                    teamStats[name] = new TeamStandingRow { Team = name };
            }

            foreach (var m in matches.Where(m => m.HomeScore.HasValue && m.AwayScore.HasValue))
            {
                EnsureTeam(m.HomeTeam);
                EnsureTeam(m.AwayTeam);

                var home = teamStats[m.HomeTeam];
                var away = teamStats[m.AwayTeam];

                home.Played++; away.Played++;
                home.GoalsFor += m.HomeScore!.Value;
                home.GoalsAgainst += m.AwayScore!.Value;
                away.GoalsFor += m.AwayScore!.Value;
                away.GoalsAgainst += m.HomeScore!.Value;

                if (m.HomeScore > m.AwayScore) { home.Won++; away.Lost++; }
                else if (m.HomeScore < m.AwayScore) { home.Lost++; away.Won++; }
                else { home.Drawn++; away.Drawn++; }
            }

            foreach (var m in matches)
            {
                EnsureTeam(m.HomeTeam);
                EnsureTeam(m.AwayTeam);
            }

            return new GroupStandingViewModel
            {
                GroupName = groupName,
                Standings = teamStats.Values
                    .OrderByDescending(t => t.Points)
                    .ThenByDescending(t => t.GoalDifference)
                    .ThenByDescending(t => t.GoalsFor)
                    .ToList()
            };
        }
    }
}