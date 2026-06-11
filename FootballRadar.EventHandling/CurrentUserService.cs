using Microsoft.AspNetCore.Http;

namespace FootballRadar.EventHandling
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId =>
            Guid.Parse(httpContextAccessor.HttpContext!.User.FindFirst("sub")!.Value);
    }
}
