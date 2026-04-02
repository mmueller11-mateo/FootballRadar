namespace FootballRadar.DataCollector.Kaggle
{
    public class TransferRecord
    {
        public int PlayerId { get; set; }
        public DateTime TransferDate { get; set; }
        public required string TransferSeason { get; set; }

        public int FromClubId { get; set; }
        public int ToClubId { get; set; }

        public required string FromClubName { get; set; }
        public required string ToClubName { get; set; }

        public decimal TransferFee { get; set; }
        public decimal MarketValueInEur { get; set; }

        public required string PlayerName { get; set; }
    }
}
