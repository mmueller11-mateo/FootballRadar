using FootballRadar.Abstractions;
using Microsoft.AspNetCore.SignalR;
using My.Framework.EventHandling;

namespace FootballRadar.EventHandling
{
    internal sealed class PushNotificationService : IEventHandler
    {
        private readonly IHubContext<EventNotificationHub> eventHub;
        private readonly IUserRepository userRepository;
        private readonly ICurrentUserService currentUserService;

        public PushNotificationService(
            IHubContext<EventNotificationHub> eventHub,
            ICurrentUserService currentUserService,
            IUserRepository userRepository)
        {
            this.eventHub = eventHub;
            this.userRepository = userRepository;
            this.currentUserService = currentUserService;
        }

        public async Task Handle<TEvent>(TEvent @event, CancellationToken cancellationToken)
            where TEvent : IEvent
        {
            var userProfile = await userRepository.GetProfileByUserIdAsync(currentUserService.UserId, cancellationToken);

            if (userProfile is null)
                return;

            var preference = userProfile.EventNotificationPreferences.SingleOrDefault(x => x.EventType == typeof(TEvent).FullName && x.IsEnabled);


            if (preference is not null)
            {
                var message = EventSerializer.Serialize(@event);

                await eventHub.Clients.User(userProfile.UserId.ToString())
                    .SendAsync("ReceiveEvent",
                        typeof(TEvent).Name,
                        message,
                        cancellationToken);
            }
        }
    }
}