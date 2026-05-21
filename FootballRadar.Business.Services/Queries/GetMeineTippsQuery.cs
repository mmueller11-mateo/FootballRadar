using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public sealed class GetMeineTippsQuery : IRequest<IEnumerable<MeinTippEntry>>
    {
        public required Guid UserId { get; set; }
    }
}
