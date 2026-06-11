using FootballRadar.Abstractions;
using FootballRadar.Business.Services.Commands;
using MediatR;

namespace FootballRadar.Business.Services.CommandHandlers
{
    internal sealed class UpdateNotificationPreferencesCommandHandler : IRequestHandler<UpdateNotificationPreferencesCommand>
    {
        private readonly IUserProfileRepository userProfileRepository;

        public UpdateNotificationPreferencesCommandHandler(IUserProfileRepository userProfileRepository)
        {
            this.userProfileRepository = userProfileRepository;
        }

        public async Task Handle(UpdateNotificationPreferencesCommand request, CancellationToken cancellationToken)
        {
            var profile = await userProfileRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (profile is null)
                throw new InvalidOperationException("User profile not found.");

            profile.EventNotificationPreferences = request.Preferences;

            await userProfileRepository.UpdateAsync(profile, cancellationToken);
        }
    }
}