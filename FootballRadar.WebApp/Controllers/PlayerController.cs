using FootballRadar.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.WebApp.Controllers
{
    public class PlayerController : Controller
    {
        public async Task<IActionResult> PlayerList(int apiTeamId, int season, CancellationToken cancellationToken = default)
        {
            var viewModelRepository = HttpContext.RequestServices.GetRequiredService<IViewModelRepository>();
            var vm = await viewModelRepository.CreateTeamPlayersViewModel(apiTeamId, season, cancellationToken);
            return View(vm);
        }
    }
}