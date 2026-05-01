using FootballRadar.Business.Entities;

namespace FootballRadar.Abstractions
{
    public interface ICurrencyConverter
    {
        Task<decimal> ConvertToCredits(Money amount);
        IEnumerable<string> SupportedCurrencies { get; }
    }
}