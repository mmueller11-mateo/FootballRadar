using FootballRadar.Business.Entities;

namespace FootballRadar.Admin.Abstractions
{
    public interface ICountryRepository
    {
        Task AddAsync(Country country);
        Task<IEnumerable<Country>> GetAllAsync();
        Task<Country?> GetByIdAsync(Guid id);
        Task<Country?> GetByNameAsync(string name);
        void Update(Country country);
        void Delete(Country country);
    }
}
