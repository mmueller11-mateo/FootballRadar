using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Services.Commands;
using MediatR;

namespace FootballRadar.Business.Services.CommandHandlers
{
    internal class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResult>
    {
        private readonly IUserRepository userRepository;
        private readonly IPasswordHasher passwordHasher;
        private readonly IWalletRepository walletRepository;
        private readonly IUserProfileRepository userProfileRepository;

        public RegisterCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IWalletRepository walletRepository,
            IUserProfileRepository userProfileRepository)
        {
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
            this.walletRepository = walletRepository;
            this.userProfileRepository = userProfileRepository;
        }

        public async Task<RegisterResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existing = await userRepository.GetByEmailAsync(request.Email);
            if (existing is not null)
                return new RegisterResult { Error = "Email already in use" };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                PasswordHash = passwordHasher.Hash(request.Password)
            };

            await userRepository.AddAsync(user);

            var wallet = new Wallet(user.Id);
            await walletRepository.AddAsync(wallet);

            var profile = new UserProfile
            {
                UserId = user.Id,
                Nickname = request.Name,
                EventNotificationPreferences = []
            };
            await userProfileRepository.AddAsync(profile, cancellationToken);

            return new RegisterResult { User = user };
        }
    }
}