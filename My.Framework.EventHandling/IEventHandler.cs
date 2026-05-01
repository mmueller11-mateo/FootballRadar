namespace My.Framework.EventHandling
{
    public interface IEventHandler
    {
        Task Handle<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : IEvent;
    }
}
