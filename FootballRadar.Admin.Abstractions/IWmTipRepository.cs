namespace FootballRadar.Admin.Abstractions
{
    public interface IWmTipRepository
    {
        Task<IEnumerable<WmTip>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<IEnumerable<WmTip>> GetByMatchIdAsync(Guid matchId, CancellationToken cancellationToken);
        Task UpdateAsync(WmTip tip, CancellationToken cancellationToken);
        Task SaveTipAsync(WmTip tip, CancellationToken cancellationToken);
    }
}
