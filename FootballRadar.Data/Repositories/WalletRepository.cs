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

        public async Task AddAsync(Wallet wallet)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            await dbContext.Wallets.AddAsync(wallet);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Wallet?> GetByUserIdAsync(Guid userId)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            return await dbContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
        }

        public async Task<Wallet?> GetByIdAsync(Guid id)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            return await dbContext.Wallets.FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task UpdateAsync(Wallet wallet)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            await dbContext.Wallets
                .Where(w => w.Id == wallet.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(w => w.Credits, wallet.Credits)
                    .SetProperty(w => w.LastUpdated, wallet.LastUpdated));
        }

        public async Task<IEnumerable<WalletTransaction>> GetTransactionsByWalletIdAsync(Guid walletId)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            return await dbContext.WalletTransactions
                .Where(t => t.WalletId == walletId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
    }
}