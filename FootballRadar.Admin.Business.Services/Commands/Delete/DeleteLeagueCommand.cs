using MediatR;

namespace FootballRadar.Admin.Business.Services.Commands.Delete
{
    public sealed class DeleteLeagueCommand : IRequest<bool>
    {
        public required Guid Id { get; init; }
    }
}
