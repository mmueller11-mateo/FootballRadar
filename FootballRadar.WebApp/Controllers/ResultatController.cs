using FootballRadar.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.WebApp.Controllers
{
    [Authorize]
    public class ResultatController : Controller
    {
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var viewModelRepository = HttpContext.RequestServices.GetRequiredService<IViewModelRepository>();
            var vm = await viewModelRepository.CreateResultatIndexViewModel(cancellationToken);
            return View(vm);
        }
    }
}