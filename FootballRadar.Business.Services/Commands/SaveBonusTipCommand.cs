using MediatR;

namespace FootballRadar.Business.Services.Commands
{
    public class SaveBonusTipCommand : IRequest
    {
        public Guid UserId { get; set; }
        public Guid BonusQuestionId { get; set; }
        public Guid AnswerTeamId { get; set; }
    }
}
