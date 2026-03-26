using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

FootballRadar.Admin.Business.Services.ComponentConfigurator.ConfigureServices(builder.Services, builder.Configuration, builder.Environment);
FootballRadar.Admin.Data.ComponentConfigurator.ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();  // falls vorhanden
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
