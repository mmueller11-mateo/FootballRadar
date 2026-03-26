using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public sealed class GetSeasonsByLeagueQuery : IRequest<IReadOnlyCollection<int>>
    {
        public required int ApiLeagueId { get; init; }
    }
}