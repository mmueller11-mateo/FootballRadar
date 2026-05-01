using My.Framework.EventHandling;

namespace FootballRadar.Abstractions.Events
{
    public sealed class TransferRumorReported : IEvent
    {
        public required Guid TransferRumorId { get; init; }
    }
}
