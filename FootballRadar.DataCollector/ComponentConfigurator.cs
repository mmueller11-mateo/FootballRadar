using FootballRadar.DataCollector.FootballAPI;
using FootballRadar.DataCollector.Services;
using FootballRadar.DataCollector.Services.FootballRadar.DataCollector.Services;
using FootballRadar.DataCollector.Services.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Refit;

namespace FootballRadar.DataCollector
{
    public static class ComponentConfigurator
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            services.AddDbContextFactory<DataCollectorDbContext>((sp, options) =>
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder(configuration.GetConnectionString("ApplicationDB"));
                options.UseSqlServer(connectionStringBuilder.ToString(), sqlServer =>
                {
                    sqlServer.EnableRetryOnFailure(maxRetryCount: 3);
                });

                options.EnableSensitiveDataLogging(hostEnvironment.IsDevelopment());
                options.EnableThreadSafetyChecks(hostEnvironment.IsDevelopment());
                options.EnableDetailedErrors(hostEnvironment.IsDevelopment());
            });

            services.AddRefitClient<IApiSportsClient>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri("https://v3.football.api-sports.io");
                    c.DefaultRequestHeaders.Add("x-apisports-key", configuration["ApiSports:ApiKey"]);
                });
            services.AddHostedService<Worker>();
            services.AddSingleton<ILeagueService, LeagueService>();
            services.AddSingleton<ICountryService, CountryService>();
            services.AddSingleton<ITeamService, TeamService>();
            services.AddSingleton<IFixtureService, FixtureService>();
        }
    }
}