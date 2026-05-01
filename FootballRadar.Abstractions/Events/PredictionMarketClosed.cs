using My.Framework.EventHandling;

namespace FootballRadar.Abstractions.Events
{
    [EventHandler(DispatchType = EventDispatchType.PushNotification)]
    public sealed class PredictionMarketClosed : IEvent
    {
    }
}
