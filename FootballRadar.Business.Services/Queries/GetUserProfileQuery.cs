using FootballRadar.Business.Entities.Betting;
using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public sealed class GetUserProfileQuery : IRequest<UserProfile>
    {
        public required Guid UserId { get; set; }
    }
}
