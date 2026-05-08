using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Data.Repositories
{
    sealed class UserRepository : IUserRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> dbContextFactory;

        public UserRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            await dbContext.Users.AddAsync(user, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            return await dbContext.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower(), cancellationToken);
        }

        public void Update(User user)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            dbContext.Users.Update(user);
            dbContext.SaveChanges();
        }

        public void Delete(User user)
        {
            using var dbContext = dbContextFactory.CreateDbContext();
            dbContext.Users.Remove(user);
            dbContext.SaveChanges();
        }

        public Task<UserProfile> GetCurrentUserProfile(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}