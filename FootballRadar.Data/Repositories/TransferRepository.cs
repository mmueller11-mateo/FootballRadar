using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Enums;
using FootballRadar.Business.Entities.TransferEntities;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Data.Repositories
{
    internal class TransferRepository : ITransferRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> dbContextFactory;

        public TransferRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public Task AddTransferRumor(TransferRumor transferRumor)
        {
            throw new NotImplementedException();
        }

        public async Task<TransferRumor?> GetTransferRumorById(Guid id)
        {
            using var db = await dbContextFactory.CreateDbContextAsync();
            return await db.TransferRumors.SingleOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<TransferRumor>> GetTransferRumors(RumorStatus rumorStatus)
        {
            using var db = await dbContextFactory.CreateDbContextAsync();
            return await db.TransferRumors.Where(x => x.Status == RumorStatus.Open).ToListAsync();
        }
    }
}
