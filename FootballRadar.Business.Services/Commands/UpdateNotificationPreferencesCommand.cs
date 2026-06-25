using FootballRadar.Business.Entities.Betting;
using MediatR;

namespace FootballRadar.Business.Services.Commands
{
    public sealed class UpdateNotificationPreferencesCommand : IRequest
    {
        public required Guid UserId { get; init; }
        public required ICollection<NotificationPreference> Preferences { get; init; }
    }

}
