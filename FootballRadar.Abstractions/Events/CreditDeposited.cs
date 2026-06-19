using My.Framework.EventHandling;

namespace FootballRadar.Abstractions.Events
{
    [EventHandler(DispatchType = EventDispatchType.Email)]
    public sealed class CreditDeposited : IEvent
    {
        public required Guid UserId { get; init; }
        public required decimal Credits { get; init; }
    }
}