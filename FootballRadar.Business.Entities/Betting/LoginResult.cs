namespace FootballRadar.Business.Entities.Betting
{
    public class LoginResult
    {
        public User? User { get; set; }
        public bool Success => User != null;
    }
}
