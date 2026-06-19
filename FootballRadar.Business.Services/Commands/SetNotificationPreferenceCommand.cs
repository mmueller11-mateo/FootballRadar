using MediatR;

namespace FootballRadar.Business.Services.Commands
{
    public sealed class SetNotificationPreferenceCommand : IRequest
    {
        public required Guid UserId { get; init; }
        public required string EventType { get; init; }
        public required bool IsEnabled { get; init; }
    }
}