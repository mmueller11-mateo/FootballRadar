using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Entities.Betting.Enums;

namespace FootballRadar.Abstractions
{
    public interface IWalletTransactionRepository
    {
        Task AddAsync(WalletTransaction transaction, CancellationToken cancellationToken = default);
        Task<IEnumerable<WalletTransaction>> GetByWalletIdAsync(Guid walletId, CancellationToken cancellationToken = default);
        Task UpdateAsync(WalletTransaction transaction, CancellationToken cancellationToken = default);
        Task<IEnumerable<WalletTransaction>> GetTransactionsAsync(WalletTransactionStatus status, CancellationToken cancellationToken = default);
        Task UpdateStatusAsync(Guid transactionId, WalletTransactionStatus status, CancellationToken cancellationToken = default);
        Task<WalletTransaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
