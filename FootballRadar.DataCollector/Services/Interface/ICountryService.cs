using FootballRadar.DataCollector.FootballAPI.Models.Country;

namespace FootballRadar.DataCollector.Services.Interface
{
    public interface ICountryService
    {
        Task<IReadOnlyCollection<CountryResponse>> GetCountriesAsync();
    }
}
