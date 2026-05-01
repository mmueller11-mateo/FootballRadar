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

        public async Task AddAsync(WalletTransaction transaction)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            await db.WalletTransactions.AddAsync(transaction);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<WalletTransaction>> GetByWalletIdAsync(Guid walletId)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            return await db.WalletTransactions
                .Where(t => t.WalletId == walletId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateAsync(WalletTransaction transaction)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            db.WalletTransactions.Update(transaction);
            await db.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(Guid transactionId, WalletTransactionStatus status)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            await db.WalletTransactions
                .Where(t => t.Id == transactionId)
                .ExecuteUpdateAsync(s => s.SetProperty(t => t.Status, status));
        }

        public async Task<IEnumerable<WalletTransaction>> GetTransactionsAsync(WalletTransactionStatus status)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            return await db.WalletTransactions
                .Where(t => t.Status == status)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<WalletTransaction?> GetByIdAsync(Guid id)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            return await db.WalletTransactions.FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}