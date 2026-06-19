using FootballRadar.Abstractions;
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

            profile.EventNotificationPreferences[request.EventType] = request.IsEnabled;
            await userProfileRepository.UpdateAsync(profile, cancellationToken);
        }
    }
}