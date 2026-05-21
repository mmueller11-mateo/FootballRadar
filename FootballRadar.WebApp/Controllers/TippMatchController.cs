using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Enums;
using FootballRadar.Business.Entities.LeagueEntities;
using FootballRadar.Business.Entities.TeamEntities;
using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.Business.Services.Commands;
using FootballRadar.Business.Services.Queries;
using FootballRadar.WebApp.Models;
using FootballRadar.WebApp.Models.FootballRadar.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FootballRadar.WebApp.Controllers
{
    [Authorize]
    public class TippMatchController : Controller
    {
        private readonly IMediator mediator;
        private readonly INationalTeamRepository nationalTeamRepository;
        private readonly IWmTipRepository wmTipRepository;

        private Guid CurrentUserId =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private string CurrentUserName =>
            User.FindFirstValue(ClaimTypes.Name)!;

        public TippMatchController(
            IMediator mediator,
            INationalTeamRepository nationalTeamRepository,
            IWmTipRepository wmTipRepository)
        {
            this.mediator = mediator;
            this.nationalTeamRepository = nationalTeamRepository;
            this.wmTipRepository = wmTipRepository;
        }

        // ========================= INDEX =========================
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var matches = await mediator.Send(new GetMatchesQuery(), cancellationToken);
            var teams = await nationalTeamRepository.GetAllAsync(cancellationToken);
            var tips = await wmTipRepository.GetByUserIdAsync(CurrentUserId, cancellationToken);

            var wmMatches = matches
                .Where(m => m.WmPhase == WmPhase.Group)
                .Select(m => ToViewModel(m, teams))
                .ToList();

            var groups = wmMatches
                .GroupBy(m => m.WmGroup)
                .OrderBy(g => g.Key)
                .Select(g => BuildGroupStanding(
                    g.Key!,
                    g.ToList(),
                    tips.ToList(),
                    teams))
                .ToList();

            return View(new WmIndexViewModel
            {
                TipperName = CurrentUserName,
                Groups = groups
            });
        }

        // ========================= GROUP =========================
        public async Task<IActionResult> Group(string groupName, CancellationToken cancellationToken)
        {
            var matches = await mediator.Send(new GetMatchesQuery(), cancellationToken);
            var teams = await nationalTeamRepository.GetAllAsync(cancellationToken);
            var tips = await wmTipRepository.GetByUserIdAsync(CurrentUserId, cancellationToken);

            var now = DateTimeOffset.UtcNow;

            var groupMatches = matches
                .Where(m => m.WmGroup == groupName)
                .Select(m => ToViewModel(m, teams))
                .OrderBy(m => m.KickoffUtc)
                .ToList();

            var groupStandings = BuildGroupStanding(
                groupName,
                groupMatches,
                tips.ToList(),
                teams);

            return View(new WmGroupDetailViewModel
            {
                GroupName = groupName,
                TipperName = CurrentUserName,
                PastMatches = groupMatches.Where(m => m.KickoffUtc <= now).ToList(),
                UpcomingMatches = groupMatches.Where(m => m.KickoffUtc > now).ToList(),
                ExistingTips = tips.ToList(),
                GroupStandings = groupStandings.Standings
            });
        }

        [HttpGet]
        public async Task<IActionResult> GroupStandings(string groupName, CancellationToken ct)
        {
            var matches = await mediator.Send(new GetMatchesQuery(), ct);
            var teams = await nationalTeamRepository.GetAllAsync(ct);
            var tips = await wmTipRepository.GetByUserIdAsync(CurrentUserId, ct);

            var groupMatches = matches
                .Where(m => m.WmGroup == groupName)
                .Select(m => ToViewModel(m, teams))
                .ToList();

            var standings = BuildGroupStanding(groupName, groupMatches, tips.ToList(), teams);

            return PartialView("_GroupStandings", standings.Standings);
        }

        [HttpGet]
        public async Task<IActionResult> Tip(Guid matchId, CancellationToken cancellationToken)
        {
            var matches = await mediator.Send(new GetMatchesQuery(), cancellationToken);
            var match = matches.FirstOrDefault(m => m.Id == matchId);

            if (match == null || match.Date <= DateTimeOffset.UtcNow)
                return RedirectToAction(nameof(Index));

            var tips = await wmTipRepository.GetByUserIdAsync(CurrentUserId, cancellationToken);
            var existing = tips.FirstOrDefault(t => t.WmMatchId == matchId);

            return View(new WmTipViewModel
            {
                MatchId = matchId,
                PredictedHome = existing?.HomeGoals ?? 0,
                PredictedAway = existing?.AwayGoals ?? 0
            });
        }

        [HttpPost]
        public async Task<IActionResult> Tip(WmTipViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(model);

            var matches = await mediator.Send(new GetMatchesQuery(), cancellationToken);
            var match = matches.FirstOrDefault(m => m.Id == model.MatchId);

            if (match == null || match.Date <= DateTimeOffset.UtcNow)
                return RedirectToAction(nameof(Index));

            await mediator.Send(new SaveWmTipCommand
            {
                UserId = CurrentUserId,
                MatchId = model.MatchId,
                HomeGoals = model.PredictedHome,
                AwayGoals = model.PredictedAway
            }, cancellationToken);

            return RedirectToAction(nameof(Group), new { groupName = match.WmGroup });
        }

        private static WmMatchViewModel ToViewModel(Match m, IEnumerable<NationalTeam> teams)
        {
            var home = teams.FirstOrDefault(t => t.Id == m.HomeNationalTeamId);
            var away = teams.FirstOrDefault(t => t.Id == m.AwayNationalTeamId);

            return new WmMatchViewModel
            {
                Id = m.Id,
                HomeTeam = home?.Name ?? "Unknown",
                AwayTeam = away?.Name ?? "Unknown",
                HomeLogoUrl = home?.Logo ?? "",
                AwayLogoUrl = away?.Logo ?? "",
                KickoffUtc = m.Date,
                WmGroup = m.WmGroup,
                WmPhase = m.WmPhase!.Value,
                HomeGoals = m.HomeGoals,
                AwayGoals = m.AwayGoals
            };
        }

        // ========================= CORE LOGIC =========================
        private static (int home, int away, bool isTip, bool isReal)?
            ResolveResult(WmMatchViewModel match, IEnumerable<WmTip> tips)
        {
            var tip = tips.FirstOrDefault(t => t.WmMatchId == match.Id);

            if (tip != null)
                return (tip.HomeGoals, tip.AwayGoals, true, false);

            if (match.HomeGoals.HasValue && match.AwayGoals.HasValue)
                return (match.HomeGoals.Value, match.AwayGoals.Value, false, true);

            return null;
        }

        // ========================= INTERACTIVE STANDINGS =========================
        private static WmGroupStandingViewModel BuildGroupStanding(
            string groupName,
            List<WmMatchViewModel> matches,
            List<WmTip> tips,
            IEnumerable<NationalTeam> teams)
        {
            var teamStats = new Dictionary<string, WmTeamStandingRow>();

            string GetLogo(string teamName)
            {
                return teams.FirstOrDefault(t => t.Name == teamName)?.Logo ?? "";
            }

            void Ensure(string team)
            {
                if (!teamStats.ContainsKey(team))
                {
                    teamStats[team] = new WmTeamStandingRow
                    {
                        Team = team,
                        Logo = GetLogo(team)
                    };
                }
            }

            foreach (var m in matches)
            {
                Ensure(m.HomeTeam);
                Ensure(m.AwayTeam);

                var result = ResolveResult(m, tips);

                if (result == null)
                    continue;

                var (homeGoals, awayGoals, isTip, isReal) = result.Value;

                var home = teamStats[m.HomeTeam];
                var away = teamStats[m.AwayTeam];

                home.Played++;
                away.Played++;

                home.GoalsFor += homeGoals;
                home.GoalsAgainst += awayGoals;

                away.GoalsFor += awayGoals;
                away.GoalsAgainst += homeGoals;

                if (homeGoals > awayGoals)
                {
                    home.Won++;
                    away.Lost++;
                }
                else if (homeGoals < awayGoals)
                {
                    home.Lost++;
                    away.Won++;
                }
                else
                {
                    home.Drawn++;
                    away.Drawn++;
                }

                home.IsPredicted |= isTip;
                away.IsPredicted |= isTip;
            }

            var ordered = teamStats.Values
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.GoalDifference)
                .ThenByDescending(t => t.GoalsFor)
                .Select((t, i) =>
                {
                    t.Position = i + 1;
                    return t;
                })
                .ToList();

            return new WmGroupStandingViewModel
            {
                GroupName = groupName,
                Matches = matches,
                Standings = ordered
            };
        }
    }
}