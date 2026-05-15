using FootballRadar.Tippspiel.Data.Repositories;
using FootballRadar.TippSpiel.Abstractions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FootballRadar.TippSpiel.Data
{
    public static class ComponentConfigurator
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddDbContextFactory<TippSpielDbContext>((sp, options) =>
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder(configuration.GetConnectionString("ApplicationDB"));
                options.UseSqlServer(connectionStringBuilder.ToString(), sqlServer =>
                {
                    sqlServer.EnableRetryOnFailure(maxRetryCount: 3);
                });
                options.EnableSensitiveDataLogging(environment.IsDevelopment());
                options.EnableThreadSafetyChecks(environment.IsDevelopment());
                options.EnableDetailedErrors(environment.IsDevelopment());
            });
            services.AddTransient<ITipperRepository, TipperRepository>();
            services.AddTransient<ITippMatchRepository, TippMatchRepository>();
            services.AddTransient<ITipRepository, TipRepository>();
        }
    }
}