using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Entities.Betting.Enums;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Data.Repositories
{
    sealed class WalletTransactionRepository : IWalletTransactionRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public WalletTransactionRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task AddAsync(WalletTransaction transaction, CancellationToken cancellationToken = default)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            await db.WalletTransactions.AddAsync(transaction, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<WalletTransaction>> GetByWalletIdAsync(Guid walletId, CancellationToken cancellationToken = default)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await db.WalletTransactions
                .Where(t => t.WalletId == walletId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(WalletTransaction transaction, CancellationToken cancellationToken = default)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            db.WalletTransactions.Update(transaction);
            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateStatusAsync(Guid transactionId, WalletTransactionStatus status, CancellationToken cancellationToken = default)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            await db.WalletTransactions
                .Where(t => t.Id == transactionId)
                .ExecuteUpdateAsync(s => s.SetProperty(t => t.Status, status), cancellationToken);
        }

        public async Task<IEnumerable<WalletTransaction>> GetTransactionsAsync(WalletTransactionStatus status, CancellationToken cancellationToken = default)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await db.WalletTransactions
                .Where(t => t.Status == status)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<WalletTransaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await db.WalletTransactions.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }
    }
}