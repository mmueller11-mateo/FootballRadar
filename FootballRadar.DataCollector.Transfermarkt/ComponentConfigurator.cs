using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FootballRadar.DataCollector.Kaggle
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

            services.AddHostedService<Worker>();
        }
    }
}