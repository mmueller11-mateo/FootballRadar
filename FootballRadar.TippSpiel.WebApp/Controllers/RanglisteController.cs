using FootballRadar.TippSpiel.Business.Queries;
using FootballRadar.TippSpiel.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.TippSpiel.WebApp.Controllers
{
    public class RanglisteController : Controller
    {
        private readonly IMediator mediator;

        public RanglisteController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var entries = await mediator.Send(new GetRanglisteQuery());

            var model = new RanglisteViewModel
            {
                Entries = entries.ToList()
            };

            return View(model);
        }
    }
}