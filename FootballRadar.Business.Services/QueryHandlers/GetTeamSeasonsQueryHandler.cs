using FootballRadar.Abstractions;
using FootballRadar.Business.Services.Queries;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    internal sealed class GetTeamSeasonsQueryHandler : IRequestHandler<GetTeamSeasonsQuery, IEnumerable<int>>
    {
        private readonly IPlayerRepository _playerRepository;

        public GetTeamSeasonsQueryHandler(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public async Task<IEnumerable<int>> Handle(GetTeamSeasonsQuery request, CancellationToken cancellationToken)
        {
            return await _playerRepository.GetSeasonsByTeamAsync(
                request.ApiTeamId,
                cancellationToken);
        }
    }
}