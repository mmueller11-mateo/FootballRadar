using MediatR;

namespace FootballRadar.Business.Services.Commands
{
    public sealed class UpdateProfileCommand : IRequest
    {
        public required Guid UserId { get; init; }
        public required string Nickname { get; init; }
        public string? ProfilePictureUrl { get; init; }
    }
}