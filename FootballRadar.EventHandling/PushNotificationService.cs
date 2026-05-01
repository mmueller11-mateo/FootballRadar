using FootballRadar.Abstractions;
using Microsoft.AspNetCore.SignalR;
using My.Framework.EventHandling;

namespace FootballRadar.EventHandling
{
    internal sealed class PushNotificationService : IEventHandler
    {
        private readonly IHubContext<EventNotificationHub> eventHub;
        private readonly IUserRepository userRepository;

        public PushNotificationService(IHubContext<EventNotificationHub> eventHub, IUserRepository userRepository)
        {
            this.eventHub = eventHub;
            this.userRepository = userRepository;
        }

        public async Task Handle<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : IEvent
        {
            var currentUser = await userRepository.GetCurrentUserProfile(cancellationToken);

            if (currentUser.EventNotificationPreferences.TryGetValue(typeof(TEvent).FullName!, out var isNotificationEnabled) && isNotificationEnabled == true)
            {
                var message = EventSerializer.Serialize(@event);
                await eventHub.Clients.User(currentUser.UserId.ToString()).SendAsync("ReceiveEvent", message, cancellationToken);
            }
        }
    }
}