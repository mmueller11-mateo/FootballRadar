using MediatR;

namespace FootballRadar.Admin.Business.Services.Commands.Create
{
    public class SetKnockoutMatchWinnerCommand : IRequest
    {
        public Guid MatchId { get; set; }
        public string Winner { get; set; } = "";
    }
}
