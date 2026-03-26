using MediatR;

namespace FootballRadar.Admin.Business.Services.Commands.Delete
{
    public sealed class DeleteTeamCommand : IRequest<bool>
    {
        public required Guid Id { get; init; }
    }
}
