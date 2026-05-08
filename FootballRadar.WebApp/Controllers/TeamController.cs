using FootballRadar.Abstractions;
using FootballRadar.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.WebApp.Controllers
{
    public class TeamController : Controller
    {
        private readonly ILeagueRepository leagueService;

        public TeamController(ILeagueRepository leagueService)
        {
            this.leagueService = leagueService;
        }

        public async Task<IActionResult> TeamsList(int leagueId, int season, CancellationToken cancellationToken = default)
        {
            var standings = await leagueService.GetStandingsWithDetailsAsync(leagueId, season, cancellationToken);
            var vm = new StandingsViewModel
            {
                LeagueId = leagueId,
                Season = season,
                Standings = standings.Select(s => new StandingViewModel
                {
                    Rank = s.Standing.Rank,
                    Points = s.Standing.Points,
                    GoalsDiff = s.Standing.GoalsDiff,
                    Team = new StandingTeamViewModel
                    {
                        Name = s.Team!.Name ?? "Unknown",
                        Logo = s.Team.Logo,
                        ApiTeamId = s.Team?.ApiTeamId
                    },
                    All = new StandingStatsViewModel
                    {
                        Played = s.Stats?.Played ?? 0,
                        Win = s.Stats?.Win ?? 0,
                        Draw = s.Stats?.Draw ?? 0,
                        Lose = s.Stats?.Lose ?? 0,
                        Goals = new StandingGoalsViewModel { For = 0, Against = 0 }
                    }
                })
                .OrderBy(s => s.Rank)
                .ToList()
            };

            return View(vm);
        }
    }
}