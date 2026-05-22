using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using FootballRadar.Web.Utilities;
using FootballRadar.WebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FootballRadar.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IPasswordHasher passwordHasher;
        private readonly IWalletRepository walletRepository;

        public AccountController(IUserRepository userRepository, IPasswordHasher passwordHasher, IWalletRepository walletRepository)
        {
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
            this.walletRepository = walletRepository;
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
            var user = await userRepository.GetByEmailAsync(model.Email);
            if (user is null || !passwordHasher.Verify(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return View(model);
            }

            await SignInAsync(user);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(LeagueController.LeaguesList), ControllerName.For<LeagueController>());
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

            var existing = await userRepository.GetByEmailAsync(model.Email);
            if (existing is not null)
            {
                ModelState.AddModelError(string.Empty, "Email already in use");
                return View(model);
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Email = model.Email,
                PasswordHash = passwordHasher.Hash(model.Password)
            };

            await userRepository.AddAsync(user);
            var wallet = new Wallet(user.Id);
            await walletRepository.AddAsync(wallet);
            await SignInAsync(user);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(LeagueController.LeaguesList), ControllerName.For<LeagueController>());
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.Index), ControllerName.For<HomeController>());
        }

        private async Task SignInAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}