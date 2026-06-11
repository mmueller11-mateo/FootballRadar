using FootballRadar.Business.Entities;

namespace FootballRadar.Admin.Abstractions
{
    public interface ICountryRepository
    {
        Task AddAsync(Country country, CancellationToken cancellationToken);
        Task<IEnumerable<Country>> GetAllAsync(CancellationToken cancellationToken);
        Task<Country?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Country?> GetByNameAsync(string name, CancellationToken cancellationToken);
        void Update(Country country, CancellationToken cancellationToken);
        void Delete(Country country, CancellationToken cancellationToken);
    }
}
