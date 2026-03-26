using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Entities.Betting.Enums;
using FootballRadar.Business.Services.Commands;
using MediatR;

namespace FootballRadar.Business.Services.CommandHandlers
{
    internal sealed class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, WalletTransaction>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletTransactionRepository _transactionRepository;

        public WithdrawCommandHandler(IWalletRepository walletRepository, IWalletTransactionRepository transactionRepository)
        {
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<WalletTransaction> Handle(WithdrawCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetByIdAsync(request.WalletId);
            if (wallet is null)
                throw new InvalidOperationException("Wallet not found");

            var transaction = new WalletTransaction
            {
                Id = Guid.NewGuid(),
                WalletId = wallet.Id,
                Amount = request.Amount,
                Credits = request.Credits,
                Type = WalletTransactionType.Withdraw,
                Status = WalletTransactionStatus.Pending,
                CreatedAt = DateTimeOffset.UtcNow
            };
            await _transactionRepository.AddAsync(transaction);
            return transaction;
        }
    }
}