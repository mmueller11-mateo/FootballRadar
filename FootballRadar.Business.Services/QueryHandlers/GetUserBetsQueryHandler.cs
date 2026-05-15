using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Services.Queries;
using MediatR;
namespace FootballRadar.Business.Services.QueryHandlers
{
    internal sealed class GetUserBetsQueryHandler : IRequestHandler<GetUserBetsQuery, IEnumerable<WinnerBet>>
    {
        private readonly IBetRepository _betRepository;
        public GetUserBetsQueryHandler(IBetRepository betRepository)
        {
            _betRepository = betRepository;
        }
        public async Task<IEnumerable<WinnerBet>> Handle(GetUserBetsQuery request, CancellationToken cancellationToken)
        {
            return await _betRepository.GetMatchBetsByUserIdAsync(request.UserId, cancellationToken);
        }
    }
}