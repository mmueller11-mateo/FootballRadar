namespace FootballRadar.Abstractions
{
    public interface IWmTipRepository
    {
        Task<IEnumerable<WmTip>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<IEnumerable<WmTip>> GetByMatchIdAsync(Guid matchId, CancellationToken cancellationToken);
        Task UpsertAsync(WmTip tip, CancellationToken cancellationToken);
        Task<IEnumerable<WmTip>> GetAllAsync(CancellationToken cancellationToken);
        Task<WmTip?> GetByUserAndMatchAsync(Guid userId, Guid matchId, CancellationToken ct);
        Task UpdateAsync(WmTip tip, CancellationToken ct);
        Task AddAsync(WmTip tip, CancellationToken ct);
    }
}