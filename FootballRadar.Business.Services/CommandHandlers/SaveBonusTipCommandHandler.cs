using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.TippSpiel;
using FootballRadar.Business.Services.Commands;
using FootballRadar.Data.Repositories;
using MediatR;

namespace FootballRadar.Business.Services.CommandHandlers
{
    public class SaveBonusTipCommandHandler : IRequestHandler<SaveBonusTipCommand>
    {
        private readonly IBonusQuestionRepository bonusQuestionRepository;
        private readonly IBonusTipRepository bonusTipRepository;

        public SaveBonusTipCommandHandler(IBonusQuestionRepository bonusQuestionRepository, IBonusTipRepository bonusTipRepository)
        {
            this.bonusQuestionRepository = bonusQuestionRepository;
            this.bonusTipRepository = bonusTipRepository;
        }

        public async Task Handle(SaveBonusTipCommand request, CancellationToken cancellationToken)
        {
            var question = await bonusQuestionRepository.GetByIdAsync(request.BonusQuestionId, cancellationToken)
                ?? throw new InvalidOperationException("Bonusfrage nicht gefunden.");

            if (DateTimeOffset.UtcNow >= question.Deadline)
                throw new InvalidOperationException("Die Tippzeit für diese Bonusfrage ist abgelaufen.");

            await bonusTipRepository.UpsertAsync(new BonusTip
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                BonusQuestionId = request.BonusQuestionId,
                AnswerTeamId = request.AnswerTeamId,
                SubmittedAt = DateTimeOffset.UtcNow
            }, cancellationToken);
        }
    }
}
