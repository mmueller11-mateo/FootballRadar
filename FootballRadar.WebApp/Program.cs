namespace FootballRadar.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpContextAccessor();
            FootballRadar.WebApp.ComponentConfigurator.ConfigureServices(builder.Services, builder.Configuration, builder.Environment);
            FootballRadar.Business.Services.ComponentConfigurator.ConfigureServices(builder.Services, builder.Configuration, builder.Environment);
            FootballRadar.Data.ComponentConfigurator.ConfigureServices(builder.Services, builder.Configuration, builder.Environment);
            My.Framework.EventHandling.ComponentConfigurator.ConfigureServices(builder.Services, builder.Configuration, builder.Environment);
            FootballRadar.EventHandling.ComponentConfigurator.ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                FootballRadar.Data.ComponentConfigurator.EnsureDatabase(scope.ServiceProvider);
                FootballRadar.EventHandling.ComponentConfigurator.EnsureDatabase(scope.ServiceProvider);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
