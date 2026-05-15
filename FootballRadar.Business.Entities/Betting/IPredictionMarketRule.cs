namespace FootballRadar.Business.Entities.Betting
{
    public interface IPredictionMarketRule
    {
        Task<bool> Evaluate(CancellationToken cancellationToken);
        string ErrorMessage { get; }
    }
}
