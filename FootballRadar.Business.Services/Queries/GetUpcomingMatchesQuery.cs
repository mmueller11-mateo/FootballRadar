using FootballRadar.Business.Entities.LeagueEntities;
using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public sealed class GetUpcomingMatchesQuery : IRequest<IReadOnlyCollection<Match>>
    {
    }
}
