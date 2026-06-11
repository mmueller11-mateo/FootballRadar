using FootballRadar.Business.Entities.Betting;

namespace FootballRadar.Abstractions
{
    public interface IUserProfileRepository
    {
        Task<UserProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task AddAsync(UserProfile profile, CancellationToken cancellationToken);
        Task UpdateAsync(UserProfile profile, CancellationToken cancellationToken);
    }
}