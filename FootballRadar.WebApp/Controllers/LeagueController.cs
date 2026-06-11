using FootballRadar.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.WebApp.Controllers
{
    public class LeagueController : Controller
    {
        public async Task<IActionResult> LeaguesList(CancellationToken cancellationToken = default)
        {
            var leagueRepository = HttpContext.RequestServices.GetRequiredService<ILeagueRepository>();
            var leagues = await leagueRepository.GetLeaguesAsync(cancellationToken);
            return View(leagues);
        }
    }
}
