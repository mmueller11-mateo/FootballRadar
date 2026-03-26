using FootballRadar.Business.Entities.Enums;
using FootballRadar.Business.Entities.TransferEntities;

namespace FootballRadar.Abstractions
{
    public interface ITransferRepository
    {
        Task<IReadOnlyCollection<TransferRumor>> GetTransferRumors(RumorStatus rumorStatus);
        Task<TransferRumor?> GetTransferRumorById(Guid id);
    }
}
