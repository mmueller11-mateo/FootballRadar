using FootballRadar.Business.Entities.Betting;
using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public class LoginQuery : IRequest<LoginResult>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

}
