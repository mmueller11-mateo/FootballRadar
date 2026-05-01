using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Entities.Betting.Enums;
using FootballRadar.Business.Services.Commands;
using MediatR;

namespace FootballRadar.Business.Services.CommandHandlers
{
    internal sealed class DepositCommandHandler : IRequestHandler<DepositCommand, WalletTransaction>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletTransactionRepository _walletTransactionRepository;

        public DepositCommandHandler(IWalletRepository walletRepository, IWalletTransactionRepository walletTransactionRepository)
        {
            _walletRepository = walletRepository;
            _walletTransactionRepository = walletTransactionRepository;
        }

        public async Task<WalletTransaction> Handle(DepositCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetByIdAsync(request.WalletId, cancellationToken);
            if (wallet is null)
                throw new InvalidOperationException("Wallet not found");

            var transaction = new WalletTransaction
            {
                Id = Guid.NewGuid(),
                WalletId = wallet.Id,
                Amount = request.Amount,
                Credits = request.Credits,
                Type = WalletTransactionType.Deposit,
                Status = WalletTransactionStatus.Pending,
                CreatedAt = DateTimeOffset.UtcNow
            };
            await _walletTransactionRepository.AddAsync(transaction, cancellationToken);
            return transaction;
        }
    }
}