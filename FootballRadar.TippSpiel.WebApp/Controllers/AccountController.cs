using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.TippSpiel.Abstractions;
using FootballRadar.TippSpiel.Business.Commands;
using FootballRadar.TippSpiel.Business.Queries;
using FootballRadar.TippSpiel.WebApp.Models;
using FootballRadar.Web.Utilities;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FootballRadar.TippSpiel.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator mediator;
        private readonly IPasswordHasher hasher;

        public AccountController(IMediator mediator, IPasswordHasher hasher)
        {
            this.mediator = mediator;
            this.hasher = hasher;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction(nameof(TippMatchController.Index), ControllerName.For<TippMatchController>());

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await mediator.Send(new GetTippUserByNameQuery { Name = model.Name });

            if (user == null || !hasher.Verify(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Ungültiger Name oder Passwort.");
                return View(model);
            }

            await SignInAsync(user);
            return LocalRedirect(returnUrl ?? "/");
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
            if (!ModelState.IsValid)
                return View(model);

            var existing = await mediator.Send(new GetTippUserByNameQuery { Name = model.Name });
            if (existing != null)
            {
                ModelState.AddModelError(nameof(model.Name), "Dieser Name ist bereits vergeben.");
                return View(model);
            }

            var user = await mediator.Send(new CreateTippUserCommand
            {
                Name = model.Name,
                Password = model.Password
            });

            await SignInAsync(user);
            return LocalRedirect(returnUrl ?? "/");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        private async Task SignInAsync(TippUser user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Name)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}