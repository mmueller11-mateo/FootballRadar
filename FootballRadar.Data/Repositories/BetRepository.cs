using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Data.Repositories
{
    sealed class BetRepository : IBetRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public BetRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<bool> HasUserBetOnMatchAsync(Guid userId, Guid matchId)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            return await db.Bets.AnyAsync(b => b.UserId == userId && b.PredictionMarketId == matchId);
        }

        public async Task<bool> HasUserBetOnMarketAsync(Guid userId, Guid marketId)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            return await db.Bets.AnyAsync(b => b.UserId == userId && b.PredictionMarketId == marketId);
        }

        public async Task<PredictionMarket?> FindPredictionMarketForMatchAsync(Guid matchId)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            return await db.MatchPredictionMarkets.FirstOrDefaultAsync(p => p.MatchId == matchId);
        }
        public async Task AddBetAsync(Bet bet)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync();
            await db.Bets.AddAsync(bet);
            await db.SaveChangesAsync();
        }
    }
}