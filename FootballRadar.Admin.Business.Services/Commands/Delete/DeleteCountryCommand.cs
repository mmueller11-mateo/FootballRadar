using MediatR;

namespace FootballRadar.Admin.Business.Services.Commands.Delete
{
    public sealed class DeleteCountryCommand : IRequest<bool>
    {
        public required Guid Id { get; init; }
    }
}
