using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Data.ORM;
using FootballRadar.Admin.Data.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FootballRadar.Admin.Data
{
    public static class ComponentConfigurator
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            services.AddDbContextFactory<AdminDbContext>((sp, options) =>
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

            services.AddTransient<ICountryRepository, CountryRepository>();
            services.AddTransient<ILeagueRepository, LeagueRepository>();
            services.AddTransient<INationalTeamRepository, NationalTeamRepository>();
            services.AddTransient<ITeamRepository, TeamRepository>();
            services.AddTransient<IMatchRepository, MatchRepository>();
            services.AddTransient<IWmTipRepository, WmTipRepository>();
        }

        public static void EnsureDatabase(IServiceProvider services)
        {
            var factory = services.GetRequiredService<IDbContextFactory<AdminDbContext>>();
            using var db = factory.CreateDbContext();
            db.Database.EnsureCreated();
        }
    }
}