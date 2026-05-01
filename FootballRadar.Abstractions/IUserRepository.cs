using FootballRadar.Business.Entities.Betting;

namespace FootballRadar.Abstractions
{
	public interface IUserRepository
	{
		Task AddAsync(User user);
		Task<User?> GetByIdAsync(Guid id);
		Task<User?> GetByEmailAsync(string email);
		void Update(User user);
		void Delete(User user);
		Task<UserProfile> GetCurrentUserProfile();
	}
}
