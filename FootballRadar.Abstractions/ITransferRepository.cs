using FootballRadar.Business.Entities.Enums;
using FootballRadar.Business.Entities.TransferEntities;

namespace FootballRadar.Abstractions
{
    public interface ITransferRepository
    {
        Task<IEnumerable<TransferRumor>> GetTransferRumors(RumorStatus rumorStatus, CancellationToken cancellationToken = default);
        Task<TransferRumor?> GetTransferRumorById(Guid id, CancellationToken cancellationToken = default);
        Task AddTransferRumor(TransferRumor transferRumor, CancellationToken cancellationToken = default);
    }
}
