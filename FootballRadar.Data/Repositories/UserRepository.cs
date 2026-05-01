using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Data.Repositories
{
    sealed class UserRepository : IUserRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public UserRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task AddAsync(User user)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            return await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            return await dbContext.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
        }

        public void Update(User user)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            dbContext.Users.Update(user);
            dbContext.SaveChanges();
        }

        public void Delete(User user)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            dbContext.Users.Remove(user);
            dbContext.SaveChanges();
        }

        public Task<UserProfile> GetCurrentUserProfile()
        {
            throw new NotImplementedException();
        }
    }
}