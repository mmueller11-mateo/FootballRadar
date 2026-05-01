
namespace My.Framework.EventHandling.Internals
{
	internal sealed class EventPublisher : IEventPublisher
	{
		private readonly IEventRepository eventRepository;

		public EventPublisher(IEventRepository eventRepository)
		{
			this.eventRepository = eventRepository;
		}

		public async Task Publish<TEvent>(TEvent eventData, CancellationToken cancellationToken) where TEvent : IEvent
		{
			await eventRepository.AddEvent(eventData, cancellationToken);
		}
	}
}
