using FootballRadar.Business.Entities;
using FootballRadar.Business.Entities.Betting;
using MediatR;

namespace FootballRadar.Business.Services.Commands
{
    public sealed class WithdrawCommand : IRequest<WalletTransaction>
    {
        public required Guid WalletId { get; init; }
        public required Money Amount { get; init; }
        public required decimal Credits { get; init; }
    }
}
