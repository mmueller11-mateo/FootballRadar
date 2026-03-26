using FootballRadar.Business.Entities.Betting;

namespace FootballRadar.Abstractions
{
    public interface IWalletRepository
    {
        Task AddAsync(Wallet wallet);
        Task<Wallet?> GetByIdAsync(Guid id);
        Task<Wallet?> GetByUserIdAsync(Guid userId);
        Task UpdateAsync(Wallet wallet);
        Task<IReadOnlyCollection<WalletTransaction>> GetTransactionsByWalletIdAsync(Guid walletId);
    }
}