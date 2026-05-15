using FootballRadar.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.WebApp.Controllers
{
    public class LeagueController : Controller
    {
        private readonly ILeagueRepository leagueService;

        public LeagueController(ILeagueRepository leagueService)
        {
            this.leagueService = leagueService;
        }
        public async Task<IActionResult> LeaguesList(CancellationToken cancellationToken = default)
        {
            var leagues = await leagueService.GetLeaguesAsync(cancellationToken);
            return View(leagues);
        }
    }
}
