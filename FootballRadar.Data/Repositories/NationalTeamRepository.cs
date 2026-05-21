using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.TeamEntities;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Data.Repositories
{
    internal sealed class NationalTeamRepository : INationalTeamRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> dbFactory;

        public NationalTeamRepository(IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public async Task<IEnumerable<NationalTeam>> GetAllAsync(CancellationToken cancellationToken)
        {
            await using var context = await dbFactory.CreateDbContextAsync(cancellationToken);
            return await context.NationalTeams.ToListAsync(cancellationToken);
        }

        public async Task<NationalTeam?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            await using var context = await dbFactory.CreateDbContextAsync(cancellationToken);
            return await context.NationalTeams.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }
    }
}