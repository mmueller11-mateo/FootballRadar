using FootballRadar.Business.Entities;
using MediatR;

namespace FootballRadar.Admin.Business.Services.Commands.Create
{
    public sealed class CreateCountryCommand : IRequest<Country>
    {
        public required string Name { get; init; }
        public required string Flag { get; init; }
        public required string Code { get; init; }
    }
}