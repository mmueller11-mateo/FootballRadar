using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting.Enums;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FootballRadar.Business.Services.Background
{
    internal class WalletTransactionExecuter : BackgroundService
    {
        private readonly ILogger<WalletTransactionExecuter> _logger;
        private readonly IWalletTransactionRepository _walletTransactionRepository;
        private readonly IWalletRepository _walletRepository;
        public WalletTransactionExecuter(ILogger<WalletTransactionExecuter> logger, IWalletRepository walletRepository, IWalletTransactionRepository walletTransactionRepository)
        {
            this._logger = logger;
            this._walletRepository = walletRepository;
            this._walletTransactionRepository = walletTransactionRepository;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await DoWork(stoppingToken);
            using PeriodicTimer dailyTimer = new(TimeSpan.FromSeconds(5));
            try
            {
                while (await dailyTimer.WaitForNextTickAsync(stoppingToken))
                {
                    await DoWork(stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Timed Hosted Service is stopping.");
            }
        }

        private async Task DoWork(CancellationToken cancellationToken)
        {
            var pendingTransactions = await _walletTransactionRepository.GetTransactionsAsync(WalletTransactionStatus.Pending, cancellationToken);

            foreach (var transaction in pendingTransactions)
            {
                var wallet = await _walletRepository.GetByIdAsync(transaction.WalletId, cancellationToken);
                if (wallet is null)
                {
                    _logger.LogWarning("Wallet not found for transaction {TransactionId}", transaction.Id);
                    await _walletTransactionRepository.UpdateStatusAsync(transaction.Id, WalletTransactionStatus.Failed, cancellationToken);
                    continue;
                }

                try
                {
                    if (transaction.Type.IsPositiveTransaction())
                        wallet.Deposit(transaction.Credits);
                    else
                        wallet.Withdraw(transaction.Credits);

                    await _walletRepository.UpdateAsync(wallet, cancellationToken);
                    await _walletTransactionRepository.UpdateStatusAsync(transaction.Id, WalletTransactionStatus.Completed, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to process transaction {TransactionId}", transaction.Id);
                    await _walletTransactionRepository.UpdateStatusAsync(transaction.Id, WalletTransactionStatus.Failed, cancellationToken);
                }
            }
        }
    }
}