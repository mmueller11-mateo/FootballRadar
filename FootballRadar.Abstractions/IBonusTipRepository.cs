using FootballRadar.Business.Entities.TippSpiel;

namespace FootballRadar.Data.Repositories
{
    public interface IBonusTipRepository
    {
        Task<IEnumerable<BonusTip>> GetByUserIdAsync(Guid userId, CancellationToken ct);
        Task<BonusTip?> GetByUserAndQuestionAsync(Guid userId, Guid questionId, CancellationToken ct);
        Task<IEnumerable<BonusTip>> GetByQuestionIdAsync(Guid questionId, CancellationToken ct);
        Task UpsertAsync(BonusTip tip, CancellationToken ct);
        Task UpdateAsync(BonusTip tip, CancellationToken ct);
        Task<IEnumerable<BonusTip>> GetAllAsync(CancellationToken ct);
    }
}
