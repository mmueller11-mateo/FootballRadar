using FootballRadar.Abstractions;
using FootballRadar.Abstractions.Events;
using FootballRadar.Business.Entities.Betting.Enums;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using My.Framework.EventHandling;

namespace FootballRadar.Business.Services.Background
{
    internal class WalletTransactionExecuter : BackgroundService
    {
        private readonly ILogger<WalletTransactionExecuter> _logger;
        private readonly IWalletTransactionRepository _walletTransactionRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IEventPublisher _eventPublisher;

        public WalletTransactionExecuter(
            ILogger<WalletTransactionExecuter> logger,
            IWalletRepository walletRepository,
            IWalletTransactionRepository walletTransactionRepository,
            IEventPublisher eventPublisher)
        {
            _logger = logger;
            _walletRepository = walletRepository;
            _walletTransactionRepository = walletTransactionRepository;
            _eventPublisher = eventPublisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await DoWork(stoppingToken);

            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));

            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await DoWork(stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("WalletTransactionExecuter is stopping.");
            }
        }

        private async Task DoWork(CancellationToken cancellationToken)
        {
            var pendingTransactions =
                await _walletTransactionRepository.GetTransactionsAsync(
                    WalletTransactionStatus.Pending,
                    cancellationToken);

            foreach (var transaction in pendingTransactions)
            {
                var wallet =
                    await _walletRepository.GetByIdAsync(transaction.WalletId, cancellationToken);

                if (wallet is null)
                {
                    _logger.LogWarning(
                        "Wallet not found for transaction {TransactionId}",
                        transaction.Id);

                    await _walletTransactionRepository.UpdateStatusAsync(
                        transaction.Id,
                        WalletTransactionStatus.Failed,
                        cancellationToken);

                    continue;
                }

                try
                {
                    // 1. Wallet update (kritisch)
                    if (transaction.Type.IsPositiveTransaction())
                        wallet.Deposit(transaction.Credits);
                    else
                        wallet.Withdraw(transaction.Credits);

                    await _walletRepository.UpdateAsync(wallet, cancellationToken);

                    // 2. Transaction markieren als erfolgreich (kritisch)
                    await _walletTransactionRepository.UpdateStatusAsync(
                        transaction.Id,
                        WalletTransactionStatus.Completed,
                        cancellationToken);

                    // 3. Events (NICHT kritisch für Geldfluss)
                    try
                    {
                        if (transaction.Type == WalletTransactionType.Deposit)
                        {
                            await _eventPublisher.Publish(new CreditDeposited
                            {
                                UserId = wallet.UserId,
                                Credits = transaction.Credits
                            }, cancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "Event publishing failed for transaction {TransactionId}, but transaction is already completed",
                            transaction.Id);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Failed to process transaction {TransactionId}",
                        transaction.Id);

                    await _walletTransactionRepository.UpdateStatusAsync(
                        transaction.Id,
                        WalletTransactionStatus.Failed,
                        cancellationToken);
                }
            }
        }
    }
}