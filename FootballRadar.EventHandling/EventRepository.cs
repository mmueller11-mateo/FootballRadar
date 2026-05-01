using Microsoft.EntityFrameworkCore;

using My.Framework.EventHandling;

namespace FootballRadar.EventHandling
{
	internal sealed class EventRepository : IEventRepository
	{
		private readonly IDbContextFactory<EventDbContext> dbContextFactory;

		public EventRepository(IDbContextFactory<EventDbContext> dbContextFactory)
		{
			this.dbContextFactory = dbContextFactory;
		}

		public async Task AddEvent<TEvent>(TEvent eventData, CancellationToken cancellationToken) where TEvent : IEvent
		{
			await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);

			var eventRecord = new EventRecord
			{
				Id = Guid.NewGuid(),
				CreatedAt = DateTimeOffset.UtcNow,
				EventType = typeof(TEvent).AssemblyQualifiedName,
				EventData = EventSerializer.Serialize(eventData)
			};

			db.Events.Add(eventRecord);

			await db.SaveChangesAsync(cancellationToken);
		}

		public async Task<IReadOnlyCollection<EventRecord>> LoadEvents(IReadOnlyCollection<string> eventTypes, CancellationToken cancellationToken)
		{
			await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);

			return await db.Events.Where(@event => eventTypes.Contains(@event.EventType)).ToArrayAsync(cancellationToken);
		}
	}
}
