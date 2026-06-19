var builder = WebApplication.CreateBuilder(args);
FootballRadar.Admin.Business.Services.ComponentConfigurator.ConfigureServices(builder.Services, builder.Configuration, builder.Environment);
FootballRadar.Admin.Data.ComponentConfigurator.ConfigureServices(builder.Services, builder.Configuration, builder.Environment);
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
    FootballRadar.Admin.Data.ComponentConfigurator.EnsureDatabase(scope.ServiceProvider);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();