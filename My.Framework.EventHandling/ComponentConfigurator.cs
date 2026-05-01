using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using My.Framework.EventHandling.Internals;

namespace My.Framework.EventHandling
{
    public static class ComponentConfigurator
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            services.AddTransient<IEventPublisher, EventPublisher>();
            services.AddTransient<IEventHandlerProvider, EventHandlerProvider>();
        }
    }
}
