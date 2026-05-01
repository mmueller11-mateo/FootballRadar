using FootballRadar.Business.Entities.Betting;
using System.Threading;

namespace FootballRadar.Abstractions
{
    public interface IWalletRepository
    {
        Task AddAsync(Wallet wallet, CancellationToken cancellationToken = default);
        Task<Wallet?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Wallet?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task UpdateAsync(Wallet wallet, CancellationToken cancellationToken = default);
        Task<IEnumerable<WalletTransaction>> GetTransactionsByWalletIdAsync(Guid walletId, CancellationToken cancellationToken = default);
    }
}