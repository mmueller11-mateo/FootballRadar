using FootballRadar.Abstractions;
using FootballRadar.Business.Entities;
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
    public class WalletController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Overview(CancellationToken cancellationToken = default)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();
            var currencyConverter = HttpContext.RequestServices.GetRequiredService<ICurrencyConverter>();
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var wallet = await mediator.Send(new GetWalletQuery { UserId = userId }, cancellationToken);
            var vm = new WalletOverviewViewModel
            {
                Credits = wallet?.Credits ?? 0,
                AvailableCurrencies = currencyConverter.SupportedCurrencies
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(WalletOverviewViewModel model, CancellationToken cancellationToken = default)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();
            var currencyConverter = HttpContext.RequestServices.GetRequiredService<ICurrencyConverter>();
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var wallet = await mediator.Send(new GetWalletQuery { UserId = userId }, cancellationToken);
            if (wallet is null)
                return Json(new { error = "Wallet not found." });

            var amount = new Money(model.DepositAmount, model.DepositCurrency);
            var credits = await currencyConverter.ConvertToCredits(amount);

            var transaction = await mediator.Send(new DepositCommand
            {
                WalletId = wallet.Id,
                Amount = amount,
                Credits = credits
            }, cancellationToken);

            return Json(new { transactionId = transaction.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(WalletOverviewViewModel model, CancellationToken cancellationToken = default)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();
            var currencyConverter = HttpContext.RequestServices.GetRequiredService<ICurrencyConverter>();

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var wallet = await mediator.Send(new GetWalletQuery { UserId = userId }, cancellationToken);
            if (wallet is null)
                return Json(new { error = "Wallet not found." });

            var amount = new Money(model.WithdrawAmount, model.WithdrawCurrency);
            var credits = await currencyConverter.ConvertToCredits(amount);

            var transaction = await mediator.Send(new WithdrawCommand
            {
                WalletId = wallet.Id,
                Amount = amount,
                Credits = credits
            }, cancellationToken);

            return Json(new { transactionId = transaction.Id });
        }

        [HttpGet]
        public async Task<IActionResult> GetBalance(CancellationToken cancellationToken = default)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var wallet = await mediator.Send(new GetWalletQuery { UserId = userId }, cancellationToken);
            return Json(new { credits = wallet?.Credits ?? 0 });
        }

        [HttpGet]
        public async Task<IActionResult> TransactionStatus(Guid id, CancellationToken cancellationToken = default)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var transaction = await mediator.Send(new GetTransactionQuery { TransactionId = id }, cancellationToken);
            var wallet = await mediator.Send(new GetWalletQuery { UserId = userId }, cancellationToken);
            return Json(new { status = transaction?.Status.ToString(), credits = wallet?.Credits ?? 0 });
        }
    }
}