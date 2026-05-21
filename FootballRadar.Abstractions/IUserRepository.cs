using FootballRadar.Business.Entities.Betting;

namespace FootballRadar.Abstractions
{
    public interface IUserRepository
    {
        Task AddAsync(User user, CancellationToken cancellationToken = default);
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        void Update(User user);
        void Delete(User user);
        Task<UserProfile> GetCurrentUserProfile(CancellationToken cancellationToken = default);
    }
}
