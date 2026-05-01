namespace My.Framework.EventHandling
{
	public interface IEventRepository
	{
		Task AddEvent<TEvent>(TEvent eventData, CancellationToken cancellationToken) where TEvent : IEvent;

		Task<IReadOnlyCollection<EventRecord>> LoadEvents(IReadOnlyCollection<string> eventTypes, CancellationToken cancellationToken);
	}
}
