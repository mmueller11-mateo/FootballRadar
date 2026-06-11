using MediatR;

namespace FootballRadar.Business.Services.Commands
{
    public class SaveWmTipCommand : IRequest
    {
        public Guid UserId { get; set; }
        public Guid MatchId { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
    }
}