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

        // Wird per fetch() vom Toggle-Switch aufgerufen - speichert sofort, kein Page Reload.
        [HttpPost]
        public async Task<IActionResult> SetNotificationPreference(
            [FromBody] SetNotificationPreferenceRequest request,
            CancellationToken cancellationToken)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();
            await mediator.Send(new SetNotificationPreferenceCommand
            {
                UserId = request.UserId,
                EventType = request.EventType,
                IsEnabled = request.IsEnabled
            }, cancellationToken);
            return Ok();
        }
    }

    public sealed class SetNotificationPreferenceRequest
    {
        public Guid UserId { get; set; }
        public string EventType { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
    }
}