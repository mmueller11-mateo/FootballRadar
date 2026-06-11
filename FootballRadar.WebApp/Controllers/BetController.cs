using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting.Enums;
using FootballRadar.Business.Services.Commands;
using FootballRadar.Business.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FootballRadar.WebApp.Controllers
{
    [Authorize]
    public class BetController : Controller
    {
        private Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);


        [HttpGet]
        public async Task<IActionResult> PlaceBet(Guid matchId, CancellationToken cancellationToken)
        {
            var viewModelRepository = HttpContext.RequestServices.GetRequiredService<IViewModelRepository>();
            var vm = await viewModelRepository.CreatePlaceBetViewModel(matchId, CurrentUserId, cancellationToken);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceBet(PlaceBetViewModel vm, CancellationToken cancellationToken)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();

            if (!ModelState.IsValid)
                return View(vm);

            var status = await mediator.Send(new PlaceMatchBetCommand
            {
                UserId = CurrentUserId,
                MatchId = vm.MatchId,
                Credits = vm.Credits,
                Prediction = vm.Prediction
            }, cancellationToken);

            if (status.Code == BetStatusCode.Rejected)
            {
                ModelState.AddModelError("", status.ErrorMessage ?? "Bet rejected.");
                return View(vm);
            }

            return RedirectToAction(nameof(BetConfirmed));
        }

        [HttpGet]
        public async Task<IActionResult> MyBets(CancellationToken cancellationToken)
        {
            var viewModelRepository = HttpContext.RequestServices.GetRequiredService<IViewModelRepository>();
            var vm = await viewModelRepository.CreateUserBetsViewModel(CurrentUserId, cancellationToken);
            return View(vm);
        }

        [HttpGet]
        public IActionResult BetConfirmed() => View();
    }
}