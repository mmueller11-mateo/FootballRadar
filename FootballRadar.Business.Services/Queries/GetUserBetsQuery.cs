using FootballRadar.Business.Entities.Betting;
using MediatR;
namespace FootballRadar.Business.Services.Queries
{
    public sealed class GetUserBetsQuery : IRequest<UserBetsResult>
    {
        public required Guid UserId { get; init; }
    }
}