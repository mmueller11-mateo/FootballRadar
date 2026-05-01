using FootballRadar.Abstractions;
using FootballRadar.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FootballRadar.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Test(CancellationToken cancellationToken)
        {
            var repository = HttpContext.RequestServices.GetRequiredService<IBetRepository>();
            var result = await repository.HasUserBetOnMarketAsync(Guid.NewGuid(), Guid.NewGuid(), cancellationToken);
            return Ok();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
