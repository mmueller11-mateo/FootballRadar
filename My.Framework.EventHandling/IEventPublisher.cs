namespace My.Framework.EventHandling
{
    public interface IEventPublisher
    {
        Task Publish<TEvent>(TEvent eventData, CancellationToken cancellationToken) where TEvent : IEvent;
    }
}
