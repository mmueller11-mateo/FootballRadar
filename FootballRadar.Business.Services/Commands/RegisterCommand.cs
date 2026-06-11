using FootballRadar.Business.Entities.Betting;
using MediatR;

namespace FootballRadar.Business.Services.Commands
{
    public class RegisterCommand : IRequest<RegisterResult>
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
