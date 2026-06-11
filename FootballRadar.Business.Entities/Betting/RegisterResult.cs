namespace FootballRadar.Business.Entities.Betting
{
    public class RegisterResult
    {
        public User? User { get; set; }
        public string? Error { get; set; }
        public bool Success => Error == null;

    }
}
