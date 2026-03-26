namespace FootballRadar.Business.Entities.Betting.Enums
{
    public enum WalletTransactionType
    {
        Deposit,
        Withdraw,
        Refund,
        Payout
    }

    public static class WalletTransactionTypeExtensions
    {
        public static bool IsPositiveTransaction(this WalletTransactionType walletTransactionType)
        {
            return walletTransactionType == WalletTransactionType.Deposit ||
                walletTransactionType == WalletTransactionType.Refund ||
                walletTransactionType == WalletTransactionType.Payout;
        }
    }
}
