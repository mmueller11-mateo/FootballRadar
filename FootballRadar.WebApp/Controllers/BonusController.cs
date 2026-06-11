using FootballRadar.Abstractions;
using FootballRadar.Business.Services.Commands;
using FootballRadar.Business.Services.Queries;
using FootballRadar.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FootballRadar.WebApp.Controllers
{
    [Authorize]
    public class BonusController : Controller
    {
        private Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();
            var nationalTeamRepository = HttpContext.RequestServices.GetRequiredService<INationalTeamRepository>();

            var questionsWithTips = await mediator.Send(new GetBonusQuestionsQuery { UserId = CurrentUserId }, ct);
            var teams = await nationalTeamRepository.GetAllAsync(ct);

            var model = new BonusIndexViewModel
            {
                QuestionsWithTips = questionsWithTips.ToList(),
                Teams = teams.OrderBy(t => t.Name).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveTip([FromBody] SaveBonusTipRequest request, CancellationToken ct)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();

            await mediator.Send(new SaveBonusTipCommand
            {
                UserId = CurrentUserId,
                BonusQuestionId = request.BonusQuestionId,
                AnswerTeamId = request.AnswerTeamId
            }, ct);

            return Ok();
        }

        public record SaveBonusTipRequest(Guid BonusQuestionId, Guid AnswerTeamId);

        public async Task<IActionResult> Resolve(CancellationToken ct)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();
            var nationalTeamRepository = HttpContext.RequestServices.GetRequiredService<INationalTeamRepository>();

            var questionsWithTips = await mediator.Send(new GetBonusQuestionsQuery { UserId = CurrentUserId }, ct);
            var teams = await nationalTeamRepository.GetAllAsync(ct);

            var model = new BonusIndexViewModel
            {
                QuestionsWithTips = questionsWithTips.ToList(),
                Teams = teams.OrderBy(t => t.Name).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResolveQuestion([FromBody] ResolveQuestionRequest request, CancellationToken ct)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();

            await mediator.Send(new ResolveBonusQuestionCommand
            {
                BonusQuestionId = request.BonusQuestionId,
                CorrectAnswerTeamId = request.CorrectAnswerTeamId
            }, ct);

            return Ok();
        }

        public record ResolveQuestionRequest(Guid BonusQuestionId, Guid CorrectAnswerTeamId);
    }
}