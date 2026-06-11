using FootballRadar.Business.Entities.Betting;

namespace FootballRadar.Abstractions
{
    public interface IUserRepository
    {
        Task AddAsync(User user, CancellationToken cancellationToken = default);
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task UpdateAsync(User user, CancellationToken cancellationToken = default);
        Task DeleteAsync(User user, CancellationToken cancellationToken = default);
        Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<UserProfile?> GetProfileByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
