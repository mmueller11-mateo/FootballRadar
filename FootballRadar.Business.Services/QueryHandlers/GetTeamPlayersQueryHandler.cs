using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.PlayerIEntities;
using FootballRadar.Business.Services.Queries;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    internal sealed class GetTeamPlayersQueryHandler
    : IRequestHandler<GetTeamPlayersQuery, IEnumerable<Player>>
    {
        private readonly IPlayerRepository _playerRepository;

        public GetTeamPlayersQueryHandler(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public async Task<IEnumerable<Player>> Handle(GetTeamPlayersQuery request, CancellationToken cancellationToken)
        {
            return await _playerRepository.GetByTeamAndSeasonAsync(
                request.ApiTeamId,
                request.Season,
                cancellationToken);
        }
    }
}
