using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Data.Repositories
{
    sealed class BetRepository : IBetRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> dbContextFactory;

        public BetRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<bool> HasUserBetOnMatchAsync(Guid userId, Guid matchId, CancellationToken cancellationToken = default)
        {
            using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await db.Bets.AnyAsync(b => b.UserId == userId && b.PredictionMarketId == matchId, cancellationToken);
        }

        public async Task<bool> HasUserBetOnMarketAsync(Guid userId, Guid marketId, CancellationToken cancellationToken = default)
        {
            using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await db.Bets.AnyAsync(b => b.UserId == userId && b.PredictionMarketId == marketId, cancellationToken);
        }

        public async Task<PredictionMarket?> FindPredictionMarketForMatchAsync(Guid matchId, CancellationToken cancellationToken = default)
        {
            using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await db.MatchPredictionMarkets.FirstOrDefaultAsync(p => p.MatchId == matchId, cancellationToken);
        }

        public async Task AddBetAsync(Bet bet, CancellationToken cancellationToken = default)
        {
            using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            await db.Bets.AddAsync(bet, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<MatchBet>> GetMatchBetsByMarketIdAsync(Guid marketId, CancellationToken cancellationToken = default)
        {
            using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await db.MatchBets
                .Where(b => b.PredictionMarketId == marketId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<MatchBet>> GetMatchBetsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await db.MatchBets
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.PlacedAt)
                .ToListAsync(cancellationToken);
        }
    }
}