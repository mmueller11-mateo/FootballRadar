using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Services.Queries;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    internal class LoginQueryHandler : IRequestHandler<LoginQuery, LoginResult>
    {
        private readonly IUserRepository userRepository;
        private readonly IPasswordHasher passwordHasher;

        public LoginQueryHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
        }

        public async Task<LoginResult> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByEmailAsync(request.Email);
            if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
                return new LoginResult { User = null };

            return new LoginResult { User = user };
        }
    }
}