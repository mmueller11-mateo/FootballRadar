using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Entities.Betting.Enums;

namespace FootballRadar.Abstractions
{
    public interface IWalletTransactionRepository
    {
        Task AddAsync(WalletTransaction transaction);
        Task<IReadOnlyCollection<WalletTransaction>> GetByWalletIdAsync(Guid walletId);
        Task UpdateAsync(WalletTransaction transaction);
        Task<IReadOnlyCollection<WalletTransaction>> GetTransactionsAsync(WalletTransactionStatus status);
        Task UpdateStatusAsync(Guid transactionId, WalletTransactionStatus status);
        Task<WalletTransaction?> GetByIdAsync(Guid id);
    }
}
