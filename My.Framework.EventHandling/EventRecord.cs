namespace My.Framework.EventHandling
{
    public sealed class EventRecord
    {
        public required Guid Id { get; init; }
        public required string EventType { get; init; }
        public required string EventData { get; init; }
        public required DateTimeOffset CreatedAt { get; init; }
        public required bool IsDispatched { get; set; }
    }
}
