using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using My.Framework.EventHandling;

namespace FootballRadar.EventHandling
{
    public static class ComponentConfigurator
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            services.AddDbContextFactory<EventDbContext>((sp, options) =>
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

            services.AddTransient<IEventRepository, EventRepository>();

            // https://learn.microsoft.com/en-us/aspnet/core/signalr/hub-filters?view=aspnetcore-10.0
            services.AddSignalR(options =>
            {
            }).AddHubOptions<EventNotificationHub>(options =>
            {
            });

            services.AddKeyedTransient<IEventHandler, PushNotificationService>(EventDispatchType.PushNotification);
            services.AddKeyedTransient<IEventHandler, EmailNotificationService>(EventDispatchType.Email);
            services.AddTransient<ICurrentUserService, CurrentUserService>();
        }

        public static void EnsureDatabase(IServiceProvider services)
        {
            var factory = services.GetRequiredService<IDbContextFactory<EventDbContext>>();
            using var db = factory.CreateDbContext();
            db.Database.EnsureCreated();
        }
    }
}
