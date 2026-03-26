using FootballRadar.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.WebApp.Controllers
{
    public class LeagueController : Controller
    {
        private readonly ILeagueRepository _webleagueService;

        public LeagueController(ILeagueRepository webleagueService)
        {
            this._webleagueService = webleagueService;
        }
        public async Task<IActionResult> LeaguesList()
        {
            var leagues = await _webleagueService.GetLeaguesAsync();
            return View(leagues);
        }
    }
}
