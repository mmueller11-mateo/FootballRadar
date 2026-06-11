using FootballRadar.Business.Services.Commands;
using FootballRadar.Business.Services.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.WebApp.Controllers
{
    public class ProfileController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(Guid userId, CancellationToken cancellationToken)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();
            var profile = await mediator.Send(new GetUserProfileQuery { UserId = userId }, cancellationToken);
            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(Guid userId, string nickname, string? profilePictureUrl, CancellationToken cancellationToken)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();
            await mediator.Send(new UpdateProfileCommand
            {
                UserId = userId,
                Nickname = nickname,
                ProfilePictureUrl = profilePictureUrl
            }, cancellationToken);
            return RedirectToAction(nameof(Index), new { userId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateNotifications(Guid userId, Dictionary<string, bool> preferences, CancellationToken cancellationToken)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();
            await mediator.Send(new UpdateNotificationPreferencesCommand
            {
                UserId = userId,
                Preferences = preferences
            }, cancellationToken);
            return RedirectToAction(nameof(Index), new { userId });
        }
    }
}