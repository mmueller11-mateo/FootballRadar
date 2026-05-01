using My.Framework.EventHandling;

namespace FootballRadar.Business.Entities.Events
{
    [EventHandler(DispatchType = EventDispatchType.PushNotification)]
    public sealed class TransferConfirmed : IEvent
    {
    }
}
