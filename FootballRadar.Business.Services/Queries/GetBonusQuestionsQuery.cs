using FootballRadar.Business.Entities.TippSpiel;
using MediatR;

namespace FootballRadar.Business.Services.Queries
{
    public class GetBonusQuestionsQuery : IRequest<IEnumerable<BonusQuestionWithTip>>
    {
        public Guid UserId { get; set; }
    }
    public class BonusQuestionWithTip
    {
        public BonusQuestion Question { get; set; } = null!;
        public BonusTip? ExistingTip { get; set; }
    }
}
