using FootballRadar.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.WebApp.Controllers
{
    namespace FootballRadar.WebApp.Controllers
    {
        public class FixtureController : Controller
        {
            private readonly IMediator mediator;

            public FixtureController(IMediator mediator)
            {
                this.mediator = mediator;
            }

            [HttpGet]
            public async Task<IActionResult> Fixtures(int leagueId, int? season = null, CancellationToken cancellationToken = default)
            {
                var vm = await HttpContext.RequestServices.GetRequiredService<IViewModelRepository>().CreateFixturesViewModel(leagueId, season, cancellationToken);
                return View(vm);
            }
        }
    }
}