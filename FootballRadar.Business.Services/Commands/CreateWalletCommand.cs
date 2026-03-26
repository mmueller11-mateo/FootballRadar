using FootballRadar.Business.Entities.Betting;
using MediatR;

namespace FootballRadar.Business.Services.Commands
{
    public sealed class CreateWalletCommand : IRequest<Wallet>
    {
        public required Guid UserId { get; init; }
    }
}
