using FootballRadar.Abstractions;
using FootballRadar.Business.Entities;

namespace FootballRadar.Business.Services
{
    public sealed class CurrencyConverter : ICurrencyConverter
    {
        private static readonly Dictionary<string, decimal> _rates = new()
        {
            { "CHF", 1.0m },
            { "PEN", 0.25m },
        };

        public IReadOnlyCollection<string> SupportedCurrencies => _rates.Keys.ToList();

        public Task<decimal> ConvertToCredits(Money amount)
        {
            if (!_rates.TryGetValue(amount.Currency.ToUpper(), out var rate))
            {
                throw new InvalidOperationException($"Currency '{amount.Currency}' is not supported.");
            }

            return Task.FromResult(amount.Amount * rate);
        }
    }
}