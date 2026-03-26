using FootballRadar.Business.Entities.Betting.Enums;

namespace FootballRadar.Business.Entities.Betting
{
    public class WalletTransaction
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public Money Amount { get; set; }
        public decimal Credits { get; set; }
        public WalletTransactionType Type { get; set; }
        public WalletTransactionStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
