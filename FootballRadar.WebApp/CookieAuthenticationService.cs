using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace FootballRadar.WebApp
{
    public class CookieAuthenticationService : IWebAuthenticationService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CookieAuthenticationService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task SignInAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await httpContextAccessor.HttpContext!.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        public async Task SignOutAsync()
        {
            await httpContextAccessor.HttpContext!.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}