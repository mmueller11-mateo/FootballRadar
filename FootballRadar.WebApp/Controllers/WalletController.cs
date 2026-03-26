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
        private readonly IMediator _mediator;
        private readonly ICurrencyConverter _currencyConverter;

        public WalletController(IMediator mediator, ICurrencyConverter currencyConverter)
        {
            _mediator = mediator;
            _currencyConverter = currencyConverter;
        }

        [HttpGet]
        public async Task<IActionResult> Overview()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var wallet = await _mediator.Send(new GetWalletQuery { UserId = userId });
            var vm = new WalletOverviewViewModel
            {
                Credits = wallet?.Credits ?? 0,
                AvailableCurrencies = _currencyConverter.SupportedCurrencies
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(WalletOverviewViewModel model)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var wallet = await _mediator.Send(new GetWalletQuery { UserId = userId });
            if (wallet is null)
                return Json(new { error = "Wallet not found." });

            var amount = new Money(model.DepositAmount, model.DepositCurrency);
            var credits = await _currencyConverter.ConvertToCredits(amount);

            var transaction = await _mediator.Send(new DepositCommand
            {
                WalletId = wallet.Id,
                Amount = amount,
                Credits = credits
            });

            return Json(new { transactionId = transaction.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(WalletOverviewViewModel model)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var wallet = await _mediator.Send(new GetWalletQuery { UserId = userId });
            if (wallet is null)
                return Json(new { error = "Wallet not found." });

            var amount = new Money(model.WithdrawAmount, model.WithdrawCurrency);
            var credits = await _currencyConverter.ConvertToCredits(amount);

            var transaction = await _mediator.Send(new WithdrawCommand
            {
                WalletId = wallet.Id,
                Amount = amount,
                Credits = credits
            });

            return Json(new { transactionId = transaction.Id });
        }

        [HttpGet]
        public async Task<IActionResult> GetBalance()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var wallet = await _mediator.Send(new GetWalletQuery { UserId = userId });
            return Json(new { credits = wallet?.Credits ?? 0 });
        }

        [HttpGet]
        public async Task<IActionResult> TransactionStatus(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var transaction = await _mediator.Send(new GetTransactionQuery { TransactionId = id });
            var wallet = await _mediator.Send(new GetWalletQuery { UserId = userId });
            return Json(new { status = transaction?.Status.ToString(), credits = wallet?.Credits ?? 0 });
        }
    }
}