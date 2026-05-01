using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public sealed class GetSeasonsByLeagueQuery : IRequest<IEnumerable<int>>
    {
        public required int ApiLeagueId { get; init; }
    }
}