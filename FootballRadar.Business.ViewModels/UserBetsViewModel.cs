namespace FootballRadar.Business.ViewModels
{
    public class UserBetsViewModel
    {
        public IEnumerable<UserBetItemViewModel> OpenBets { get; set; } = new List<UserBetItemViewModel>();
        public IEnumerable<UserBetItemViewModel> SettledBets { get; set; } = new List<UserBetItemViewModel>();
        public int TotalBets { get; set; }
        public int WonBets { get; set; }
        public int LostBets { get; set; }
        public decimal TotalWinnings { get; set; }
        public decimal TotalStaked { get; set; }
    }
}
