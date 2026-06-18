using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Data.ORM;
using FootballRadar.Business.Entities.LeagueEntities;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Admin.Data.Repositories
{
    internal class MatchRepository : IMatchRepository
    {
        private readonly IDbContextFactory<AdminDbContext> context;

        public MatchRepository(IDbContextFactory<AdminDbContext> context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Match>> GetWmMatchesAsync(CancellationToken cancellationToken)
        {
            using var dbContext = await context.CreateDbContextAsync();
            return await dbContext.Fixtures
                .Where(m => m.WmPhase != null)
                .OrderBy(m => m.Date)
                .ToListAsync(cancellationToken);
        }

        public async Task<Match?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            using var dbContext = await context.CreateDbContextAsync();
            return await dbContext.Fixtures
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(Match match, CancellationToken cancellationToken)
        {
            using var dbContext = await context.CreateDbContextAsync();
            dbContext.Fixtures.Update(match);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AddAsync(Match match, CancellationToken cancellationToken)
        {
            using var dbContext = await context.CreateDbContextAsync();
            await dbContext.Fixtures.AddAsync(match, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Match>> GetByStatusAsync(string status, CancellationToken cancellationToken)
        {
            using var dbContext = await context.CreateDbContextAsync();
            return await dbContext.Fixtures
                .Where(m => m.WmPhase != null && m.Status == status)
                .ToListAsync(cancellationToken);
        }
    }
}
