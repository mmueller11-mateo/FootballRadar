using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Services.Queries;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    internal sealed class GetWalletQueryHandler : IRequestHandler<GetWalletQuery, Wallet?>
    {
        private readonly IWalletRepository _walletRepository;

        public GetWalletQueryHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<Wallet?> Handle(GetWalletQuery request, CancellationToken cancellationToken)
        {
            return await _walletRepository.GetByUserIdAsync(request.UserId);
        }
    }
}