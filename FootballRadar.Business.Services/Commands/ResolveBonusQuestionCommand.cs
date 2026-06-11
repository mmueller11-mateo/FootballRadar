using MediatR;

namespace FootballRadar.Business.Services.Commands
{
    public class ResolveBonusQuestionCommand : IRequest
    {
        public Guid BonusQuestionId { get; set; }
        public Guid CorrectAnswerTeamId { get; set; }
    }
}
