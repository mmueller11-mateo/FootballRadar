using MediatR;

namespace FootballRadar.Admin.Business.Services.Commands.Create
{
    public sealed class SetKnockoutMatchResultCommand : IRequest
    {
        public Guid MatchId { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
    }
}
