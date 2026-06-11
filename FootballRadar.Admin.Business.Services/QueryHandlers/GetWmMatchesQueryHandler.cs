using FootballRadar.Admin.Abstractions;
using FootballRadar.Admin.Business.Services.Queries;
using FootballRadar.Business.Entities.LeagueEntities;
using MediatR;

namespace FootballRadar.Admin.Business.Services.QueryHandlers
{
    public class GetWmMatchesQueryHandler : IRequestHandler<GetWmMatchesQuery, IEnumerable<Match>>
    {
        private readonly IMatchRepository matchRepository;

        public GetWmMatchesQueryHandler(IMatchRepository matchRepository)
        {
            this.matchRepository = matchRepository;
        }

        public async Task<IEnumerable<Match>> Handle(GetWmMatchesQuery request, CancellationToken cancellationToken)
        {
            return await matchRepository.GetWmMatchesAsync(cancellationToken);
        }
    }
}
