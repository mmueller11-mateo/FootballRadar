using FootballRadar.Abstractions;
using FootballRadar.Business.Services.Commands;
using FootballRadar.Data.Repositories;
using MediatR;

namespace FootballRadar.Business.Services.CommandHandlers
{
    public class ResolveBonusQuestionCommandHandler : IRequestHandler<ResolveBonusQuestionCommand>
    {
        private readonly IBonusQuestionRepository bonusQuestionRepository;
        private readonly IBonusTipRepository bonusTipRepository;

        public ResolveBonusQuestionCommandHandler(IBonusQuestionRepository bonusQuestionRepository, IBonusTipRepository bonusTipRepository)
        {
            this.bonusQuestionRepository = bonusQuestionRepository;
            this.bonusTipRepository = bonusTipRepository;
        }

        public async Task Handle(ResolveBonusQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await bonusQuestionRepository.GetByIdAsync(request.BonusQuestionId, cancellationToken)
                ?? throw new InvalidOperationException("Bonusfrage nicht gefunden.");

            // Richtige Antwort setzen
            question.CorrectAnswerTeamId = request.CorrectAnswerTeamId;
            question.IsResolved = true;
            await bonusQuestionRepository.UpdateAsync(question, cancellationToken);

            // Punkte an alle richtigen Tipper vergeben
            var tips = await bonusTipRepository.GetByQuestionIdAsync(request.BonusQuestionId, cancellationToken);
            foreach (var tip in tips)
            {
                tip.Points = tip.AnswerTeamId == request.CorrectAnswerTeamId ? question.Points : 0;
                await bonusTipRepository.UpdateAsync(tip, cancellationToken);
            }
        }
    }
}
