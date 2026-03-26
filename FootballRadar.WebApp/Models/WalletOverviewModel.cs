namespace FootballRadar.WebApp.Models
{
    public class WalletOverviewViewModel
    {
        public decimal Credits { get; set; }
        public decimal DepositAmount { get; set; }
        public string DepositCurrency { get; set; } = "CHF";
        public decimal WithdrawAmount { get; set; }
        public string WithdrawCurrency { get; set; } = "CHF";
        public IReadOnlyCollection<string> AvailableCurrencies { get; set; } = ["CHF", "PEN"];
    }
}