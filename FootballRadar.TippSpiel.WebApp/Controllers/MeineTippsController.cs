using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.TippSpiel.WebApp.Controllers
{
    public class MeineTippsController : Controller
    {
        private readonly IMediator mediator;

        public MeineTippsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}