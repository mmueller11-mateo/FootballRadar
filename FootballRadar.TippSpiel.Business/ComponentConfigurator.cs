using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FootballRadar.TippSpiel.Business
{
    public static class ComponentConfigurator
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(ComponentConfigurator).Assembly);
            });
        }
    }
}
