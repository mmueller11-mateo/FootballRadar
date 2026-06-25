using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Services.Commands;
using MediatR;

namespace FootballRadar.Business.Services.CommandHandlers
{
    internal sealed class SetNotificationPreferenceCommandHandler : IRequestHandler<SetNotificationPreferenceCommand>
    {
        private readonly IUserProfileRepository userProfileRepository;

        public SetNotificationPreferenceCommandHandler(IUserProfileRepository userProfileRepository)
        {
            this.userProfileRepository = userProfileRepository;
        }

        public async Task Handle(SetNotificationPreferenceCommand request, CancellationToken cancellationToken)
        {
            var profile = await userProfileRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (profile is null)
                throw new InvalidOperationException("User profile not found.");

            var preference = profile.EventNotificationPreferences.SingleOrDefault(x => x.EventType == request.EventType);
            if (preference is not null)
            {
                preference.IsEnabled = request.IsEnabled;
            }
            else
            {
                profile.EventNotificationPreferences.Add(new NotificationPreference
                {
                    UserId = request.UserId,
                    EventType = request.EventType,
                    IsEnabled = request.IsEnabled
                });
            }
            await userProfileRepository.UpdateAsync(profile, cancellationToken);
        }
    }
}