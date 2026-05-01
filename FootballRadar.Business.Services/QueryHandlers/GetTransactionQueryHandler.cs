using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Services.Queries;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    internal sealed class GetTransactionQueryHandler : IRequestHandler<GetTransactionQuery, WalletTransaction?>
    {
        private readonly IWalletTransactionRepository _transactionRepository;

        public GetTransactionQueryHandler(IWalletTransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<WalletTransaction?> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
        {
            return await _transactionRepository.GetByIdAsync(request.TransactionId, cancellationToken);
        }
    }
}