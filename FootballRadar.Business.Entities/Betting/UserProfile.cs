using System.ComponentModel.DataAnnotations.Schema;

namespace FootballRadar.Business.Entities.Betting
{
    public class UserProfile
    {
        public Guid UserId { get; set; }
        public string Nickname { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public ICollection<NotificationPreference> EventNotificationPreferences { get; set; } = [];

        [NotMapped]
        public IReadOnlyList<Bet> ActiveBets { get; set; } = [];
        [NotMapped]
        public IReadOnlyList<WalletTransaction> RecentTransactions { get; set; } = [];
        [NotMapped]
        public Wallet? Wallet { get; set; }
    }
}