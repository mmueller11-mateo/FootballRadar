using FootballRadar.Abstractions;
using FootballRadar.Business.Services.Commands;
using MediatR;

namespace FootballRadar.Business.Services.CommandHandlers
{
    internal sealed class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand>
    {
        private readonly IUserProfileRepository userProfileRepository;

        public UpdateProfileCommandHandler(IUserProfileRepository userProfileRepository)
        {
            this.userProfileRepository = userProfileRepository;
        }

        public async Task Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = await userProfileRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (profile is null)
                throw new InvalidOperationException("User profile not found.");

            profile.Nickname = request.Nickname;
            profile.ProfilePictureUrl = request.ProfilePictureUrl;

            await userProfileRepository.UpdateAsync(profile, cancellationToken);
        }
    }
}