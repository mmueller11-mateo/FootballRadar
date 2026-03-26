using FootballRadar.Business.Entities.TeamEntities;
using MediatR;

namespace FootballRadar.Admin.Business.Services.Commands.Create
{
    public sealed class CreateTeamCommand : IRequest<Team>
    {
        public required string Name { get; init; }
        public required Guid? CountryId { get; init; }
        public required int? ApiTeamId { get; init; }
        public required string Logo { get; init; }
    }
}
