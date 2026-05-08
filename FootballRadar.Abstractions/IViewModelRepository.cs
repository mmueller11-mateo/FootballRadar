using FootballRadar.Business.ViewModels;

namespace FootballRadar.Abstractions
{
    public interface IViewModelRepository
    {
        Task<PlaceBetViewModel> CreatePlaceBetViewModel(Guid matchId, Guid userId, CancellationToken cancellationToken);
        Task<FixturesViewModel> CreateFixturesViewModel(int leagueId, int? season, CancellationToken cancellationToken);
    }
}