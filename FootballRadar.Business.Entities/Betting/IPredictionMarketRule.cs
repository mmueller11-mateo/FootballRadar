namespace FootballRadar.Business.Entities.Betting
{
    public interface IPredictionMarketRule
    {
        Task<bool> Evaluate();
        string ErrorMessage { get; }
    }
}
