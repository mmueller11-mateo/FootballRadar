namespace My.Framework.EventHandling
{
    public interface IEventHandlerProvider
    {
        IEventHandler GetHandlerForEvent<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
