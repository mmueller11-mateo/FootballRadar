using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Data.ORM;
using FootballRadar.Business.Entities.TeamEntities;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Admin.Data.Repositories
{
    sealed class NationalTeamRepository : INationalTeamRepository
    {
        private readonly IDbContextFactory<AdminDbContext> dbContextFactory;

        public NationalTeamRepository(IDbContextFactory<AdminDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task AddAsync(NationalTeam nationalTeam)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            await dbContext.NationalTeams.AddAsync(nationalTeam);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<NationalTeam>> GetAllAsync()
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.NationalTeams.ToListAsync();
        }

        public async Task<NationalTeam?> GetByIdAsync(Guid id)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.NationalTeams.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<NationalTeam?> GetByNameAsync(string name)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.NationalTeams.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public void Update(NationalTeam nationalTeam)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            dbContext.NationalTeams.Update(nationalTeam);
            dbContext.SaveChanges();
        }

        public void Delete(NationalTeam nationalTeam)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            dbContext.NationalTeams.Remove(nationalTeam);
            dbContext.SaveChanges();
        }
    }
}