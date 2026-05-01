using FootballRadar.Abstractions;
using Microsoft.Extensions.Hosting;

using My.Framework.EventHandling;

namespace FootballRadar.EventHandling
{
    internal sealed class EventDispacher : BackgroundService
    {
        private readonly IEventHandlerProvider eventHandlerProvider;
        private readonly IUserRepository userRepository;
        private readonly IEventRepository eventRepository;
        public EventDispacher(IEventHandlerProvider eventHandlerProvider, IUserRepository userRepository, IEventRepository eventRepository)
        {
            this.eventHandlerProvider = eventHandlerProvider;
            this.userRepository = userRepository;
            this.eventRepository = eventRepository;
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
            var events = await eventRepository.LoadEvents(Array.Empty<string>(), cancellationToken);

            foreach (var eventRecord in events)
            {
                var eventData = EventSerializer.Deserialize(eventRecord.EventData, eventRecord.EventType);

                var eventHandler = eventHandlerProvider.GetHandlerForEvent(eventData);

                await eventHandler.Handle(eventData, cancellationToken);

                await eventRepository.MarkEventAsProcessed(eventRecord.Id, cancellationToken);
            }
        }
    }
}