using FootballRadar.Business.Entities.Betting;
using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public sealed class GetTransactionQuery : IRequest<WalletTransaction?>
    {
        public required Guid TransactionId { get; init; }
    }
}