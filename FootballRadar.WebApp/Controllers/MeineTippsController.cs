using FootballRadar.Business.Services.Queries;
using FootballRadar.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FootballRadar.WebApp.Controllers
{
    [Authorize]
    public class MeineTippsController : Controller
    {
        private readonly IMediator mediator;

        private Guid CurrentUserId =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public MeineTippsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var entries = await mediator.Send(new GetMeineTippsQuery { UserId = CurrentUserId }, cancellationToken);
            return View(new MeineTippsViewModel { Entries = entries.ToList() });
        }
    }
}