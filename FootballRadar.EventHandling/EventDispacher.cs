using FootballRadar.Abstractions;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

using My.Framework.EventHandling;

namespace FootballRadar.EventHandling
{
	internal sealed class EventDispacher : BackgroundService
	{
		private readonly IUserRepository userRepository;
		private static readonly Guid SystemId = Guid.Empty;

		private readonly IEventRepository eventRepository;

		// see https://learn.microsoft.com/en-us/aspnet/core/signalr/hubcontext?view=aspnetcore-10.0
		private readonly IHubContext<EventNotificationHub> eventHub;

		public EventDispacher(IUserRepository userRepository, IEventRepository eventRepository, IHubContext<EventNotificationHub> eventHub)
		{
			this.eventRepository = eventRepository;
			this.userRepository = userRepository;
			this.eventHub = eventHub;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			await DoWork(stoppingToken);

			using PeriodicTimer dailyTimer = new(TimeSpan.FromMinutes(5));

			try
			{
				while (await dailyTimer.WaitForNextTickAsync(stoppingToken))
				{
					await DoWork(stoppingToken);
				}
			}
			catch (OperationCanceledException)
			{
			}
		}

		private async Task DoWork(CancellationToken cancellationToken)
		{
			var currentUser = await userRepository.GetCurrentUserProfile();

			// here we get the list of events (by their type) that the user wants to be notified about
			var eventsOfInterest = currentUser.EventNotificationPreferences.Where(@event => @event.Value == true).Select(@event => @event.Key).ToArray();

			// here we load only the events of interest from the database
			var events = await eventRepository.LoadEvents(eventsOfInterest, cancellationToken);

			foreach (var eventRecord in events)
			{
				var eventData = EventSerializer.Deserialize(eventRecord.EventData, eventRecord.EventType);

				await eventHub.Clients.User(currentUser.UserId.ToString()).SendAsync(method: "ReceiveNotification", arg1: SystemId, arg2: eventData, cancellationToken);
			}
		}
	}
}
