using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Data.Repositories
{
    internal sealed class UserProfileRepository : IUserProfileRepository
    {
        private readonly ApplicationDbContext db;

        public UserProfileRepository(ApplicationDbContext db) => this.db = db;

        public Task<UserProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
            => db.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        public async Task AddAsync(UserProfile profile, CancellationToken cancellationToken)
        {
            await db.UserProfiles.AddAsync(profile, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(UserProfile profile, CancellationToken cancellationToken)
        {
            db.UserProfiles.Update(profile);
            await db.SaveChangesAsync(cancellationToken);
        }
    }
}