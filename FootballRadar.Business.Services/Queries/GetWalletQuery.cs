using FootballRadar.Business.Entities.Betting;
using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public sealed class GetWalletQuery : IRequest<Wallet?>
    {
        public required Guid UserId { get; init; }
    }
}