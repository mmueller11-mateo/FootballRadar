using FootballRadar.Abstractions;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FootballRadar.WebApp
{
    public static class ComponentConfigurator
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.ReturnUrlParameter = "returnUrl";
            });

            if (environment.IsDevelopment())
            {
                services.AddDatabaseDeveloperPageExceptionFilter();
            }

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddAuthorization();
            services.AddHttpContextAccessor();
            services.AddScoped<IWebAuthenticationService, CookieAuthenticationService>();
        }
    }
}