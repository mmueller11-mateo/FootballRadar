using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Data.Repositories
{
    sealed class WalletRepository : IWalletRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public WalletRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task AddAsync(Wallet wallet, CancellationToken cancellationToken = default)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            await dbContext.Wallets.AddAsync(wallet, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Wallet?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await dbContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId, cancellationToken);
        }

        public async Task<Wallet?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await dbContext.Wallets.FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(Wallet wallet, CancellationToken cancellationToken = default)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            await dbContext.Wallets
                .Where(w => w.Id == wallet.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(w => w.Credits, wallet.Credits)
                    .SetProperty(w => w.LastUpdated, wallet.LastUpdated), cancellationToken);
        }

        public async Task<IEnumerable<WalletTransaction>> GetTransactionsByWalletIdAsync(Guid walletId, CancellationToken cancellationToken = default)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await dbContext.WalletTransactions
                .Where(t => t.WalletId == walletId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}