using FootballRadar.Business.Entities.LeagueEntities;
using MediatR;

namespace FootballRadar.Admin.Business.Services.Commands.Create
{
    public sealed class CreateLeagueCommand : IRequest<PublicLeague>
    {
        public required string Name { get; init; }
        public required Guid CountryId { get; init; }
        public required int ApiLeagueId { get; init; }
    }
}
