using FootballRadar.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.WebApp.Controllers
{
    public class TeamController : Controller
    {
        public async Task<IActionResult> TeamsList(int leagueId, int season, CancellationToken cancellationToken = default)
        {
            var viewModelRepository = HttpContext.RequestServices.GetRequiredService<IViewModelRepository>();
            var vm = await viewModelRepository.CreateStandingsViewModel(leagueId, season, cancellationToken);
            return View(vm);
        }
    }
}