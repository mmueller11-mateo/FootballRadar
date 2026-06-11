using FootballRadar.Business.Entities.Betting;

namespace FootballRadar.Abstractions
{
    public interface IWebAuthenticationService
    {
        Task SignInAsync(User user);
        Task SignOutAsync();
    }
}