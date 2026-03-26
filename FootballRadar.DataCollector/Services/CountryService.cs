using FootballRadar.DataCollector.FootballAPI;
using FootballRadar.DataCollector.FootballAPI.Models.Country;
using FootballRadar.DataCollector.Services.Interface;

namespace FootballRadar.DataCollector.Services
{
    internal class CountryService : ICountryService
    {
        private readonly IApiSportsClient client;

        public CountryService(IApiSportsClient apiSportsClient)
        {
            client = apiSportsClient;
        }

        public async Task<IReadOnlyCollection<CountryResponse>> GetCountriesAsync()
        {
            var response = await client.GetCountriesAsync();
            return response.Response;
        }
    }
}
