using FootballRadar.Abstractions;
using FootballRadar.Business.Services.Commands;
using FootballRadar.Business.Services.Queries;
using FootballRadar.Web.Utilities;
using FootballRadar.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator mediator;
        private readonly IWebAuthenticationService authService;

        public AccountController(IMediator mediator, IWebAuthenticationService authService)
        {
            this.mediator = mediator;
            this.authService = authService;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction(nameof(HomeController.Index), ControllerName.For<HomeController>());

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            var result = await mediator.Send(new LoginQuery
            {
                Email = model.Email,
                Password = model.Password
            });

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return View(model);
            }

            await authService.SignInAsync(result.User!);
            return RedirectToLocal(returnUrl, nameof(LeagueController.LeaguesList), ControllerName.For<LeagueController>());
        }

        [HttpGet]
        public IActionResult Register(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
        {
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match");
                return View(model);
            }

            var result = await mediator.Send(new RegisterCommand
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password
            });

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Error!);
                return View(model);
            }

            await authService.SignInAsync(result.User!);
            return RedirectToLocal(returnUrl, nameof(LeagueController.LeaguesList), ControllerName.For<LeagueController>());
        }

        public async Task<IActionResult> Logout()
        {
            await authService.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), ControllerName.For<HomeController>());
        }

        private IActionResult RedirectToLocal(string? returnUrl, string action, string controller)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(action, controller);
        }
    }
}