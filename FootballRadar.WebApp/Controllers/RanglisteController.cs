using FootballRadar.Business.Services.Queries;
using FootballRadar.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.WebApp.Controllers
{
    public class RanglisteController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();
            var entries = await mediator.Send(new GetRanglisteQuery());

            var model = new RanglisteViewModel
            {
                Entries = entries.ToList()
            };

            return View(model);
        }
    }
}