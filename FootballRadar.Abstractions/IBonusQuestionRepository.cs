using FootballRadar.Business.Entities.TippSpiel;

namespace FootballRadar.Abstractions
{
    public interface IBonusQuestionRepository
    {
        Task<IEnumerable<BonusQuestion>> GetAllAsync(CancellationToken ct);
        Task<BonusQuestion?> GetByIdAsync(Guid id, CancellationToken ct);
        Task UpdateAsync(BonusQuestion question, CancellationToken ct);
    }
}
