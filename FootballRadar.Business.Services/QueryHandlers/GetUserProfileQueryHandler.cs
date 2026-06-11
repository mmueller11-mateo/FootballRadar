using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Services.Queries;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    internal sealed class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfile>
    {
        private readonly IUserProfileRepository userProfileRepository;
        private readonly IWalletRepository walletRepository;
        private readonly IWalletTransactionRepository walletTransactionRepository;
        private readonly IBetRepository betRepository;

        public GetUserProfileQueryHandler(IUserProfileRepository userProfileRepository, IWalletRepository walletRepository, IWalletTransactionRepository walletTransactionRepository, IBetRepository betRepository)
        {
            this.userProfileRepository = userProfileRepository;
            this.walletRepository = walletRepository;
            this.walletTransactionRepository = walletTransactionRepository;
            this.betRepository = betRepository;
        }

        public async Task<UserProfile> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var profile = await userProfileRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (profile is null)
                throw new InvalidOperationException("User profile not found.");

            var wallet = await walletRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (wallet is null)
                throw new InvalidOperationException("Wallet not found.");

            var transactions = await walletTransactionRepository.GetByWalletIdAsync(wallet.Id, cancellationToken);
            var activeBets = await betRepository.GetActiveByUserIdAsync(request.UserId, cancellationToken);

            profile.Wallet = wallet;
            profile.RecentTransactions = transactions
                .OrderByDescending(t => t.CreatedAt)
                .Take(20)
                .ToList();
            profile.ActiveBets = activeBets;

            return profile;
        }
    }
}