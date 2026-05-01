using My.Framework.EventHandling;

namespace FootballRadar.EventHandling
{
    internal sealed class EmailNotificationService : IEventHandler
    {
        public Task Handle<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : IEvent
        {
            throw new NotImplementedException();
        }
    }
}
