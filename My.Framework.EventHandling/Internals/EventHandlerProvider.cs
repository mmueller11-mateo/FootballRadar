using Microsoft.Extensions.DependencyInjection;

namespace My.Framework.EventHandling.Internals
{
    internal sealed class EventHandlerProvider : IEventHandlerProvider
    {
        private readonly IServiceProvider serviceProvider;
        public EventHandlerProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IEventHandler GetHandlerForEvent<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var attribute = Attribute.GetCustomAttribute(@event.GetType(), typeof(EventHandlerAttribute)) as EventHandlerAttribute;
            if (attribute is null)
            {
                throw new InvalidOperationException($"Event of type {@event.GetType().FullName} does not have an {nameof(EventHandlerAttribute)}.");
            }

            return serviceProvider.GetRequiredKeyedService<IEventHandler>(attribute.DispatchType);
        }
    }
}
