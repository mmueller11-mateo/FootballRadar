using FootballRadar.Abstractions;
using FootballRadar.Business.Services.Queries;
using FootballRadar.Data.Repositories;
using MediatR;

namespace FootballRadar.Business.Services.QueryHandlers
{
    public class GetBonusQuestionsQueryHandler : IRequestHandler<GetBonusQuestionsQuery, IEnumerable<BonusQuestionWithTip>>
    {
        private readonly IBonusQuestionRepository bonusQuestionRepository;
        private readonly IBonusTipRepository bonusTipRepository;

        public GetBonusQuestionsQueryHandler(IBonusQuestionRepository bonusQuestionRepository, IBonusTipRepository bonusTipRepository)
        {
            this.bonusQuestionRepository = bonusQuestionRepository;
            this.bonusTipRepository = bonusTipRepository;
        }

        public async Task<IEnumerable<BonusQuestionWithTip>> Handle(GetBonusQuestionsQuery request, CancellationToken cancellationToken)
        {
            var questions = await bonusQuestionRepository.GetAllAsync(cancellationToken);
            var tips = await bonusTipRepository.GetByUserIdAsync(request.UserId, cancellationToken);

            return questions
                .OrderBy(q => q.SortOrder)
                .Select(q => new BonusQuestionWithTip
                {
                    Question = q,
                    ExistingTip = tips.FirstOrDefault(t => t.BonusQuestionId == q.Id)
                });
        }
    }
}