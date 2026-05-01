using My.Framework.EventHandling;

namespace FootballRadar.Abstractions.Events
{
    [EventHandler(DispatchType = EventDispatchType.PushNotification)]
    public sealed class TransferRumorReported : IEvent
    {
        public required Guid TransferRumorId { get; init; }
    }
}
