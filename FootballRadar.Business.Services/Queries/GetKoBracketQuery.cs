using FootballRadar.Business.Entities.TippSpiel;
using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public sealed class GetKoBracketQuery : IRequest<KoBracketResult>
    {
        public Guid UserId { get; set; }
    }
}
