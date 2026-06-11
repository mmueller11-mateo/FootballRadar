using MediatR;

namespace FootballRadar.Business.Services.Commands
{
    public sealed class UpdateNotificationPreferencesCommand : IRequest
    {
        public required Guid UserId { get; init; }
        public required Dictionary<string, bool> Preferences { get; init; }
    }

}
