using FootballRadar.Business.Entities.Enums;
using FootballRadar.Business.Entities.TeamEntities;
using MediatR;

namespace FootballRadar.Admin.Business.Services.Commands.Create
{
    public sealed class CreateNationalTeamCommand : IRequest<NationalTeam>
    {
        public required string Name { get; init; }
        public required Guid CountryId { get; init; }
        public required NationalTeamLevel Level { get; init; }
    }
}
