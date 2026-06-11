using FootballRadar.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.WebApp.Controllers
{
    namespace FootballRadar.WebApp.Controllers
    {
        public class FixtureController : Controller
        {
            [HttpGet]
            public async Task<IActionResult> Fixtures(int leagueId, int? season = null, CancellationToken cancellationToken = default)
            {
                var vm = await HttpContext.RequestServices.GetRequiredService<IViewModelRepository>().CreateFixturesViewModel(leagueId, season, cancellationToken);
                return View(vm);
            }
        }
    }
}