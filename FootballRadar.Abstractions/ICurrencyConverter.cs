using FootballRadar.Business.Entities;

namespace FootballRadar.Abstractions
{
    public interface ICurrencyConverter
    {
        Task<decimal> ConvertToCredits(Money amount);
        IReadOnlyCollection<string> SupportedCurrencies { get; }
    }
}