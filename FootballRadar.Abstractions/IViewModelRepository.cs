using FootballRadar.Business.ViewModels;

namespace FootballRadar.Abstractions
{
    public interface IViewModelRepository
    {
        Task<PlaceBetViewModel> CreatePlaceBetViewModel(Guid matchId, Guid userId, CancellationToken cancellationToken);
        Task<FixturesViewModel> CreateFixturesViewModel(int leagueId, int? season, CancellationToken cancellationToken);
        Task<UserBetsViewModel> CreateUserBetsViewModel(Guid userId, CancellationToken cancellationToken);
        Task<TeamPlayersViewModel> CreateTeamPlayersViewModel(int apiTeamId, int season, CancellationToken cancellationToken);
        Task<ResultatIndexViewModel> CreateResultatIndexViewModel(CancellationToken cancellationToken);
        Task<StandingsViewModel> CreateStandingsViewModel(int leagueId, int season, CancellationToken cancellationToken);
    }
}