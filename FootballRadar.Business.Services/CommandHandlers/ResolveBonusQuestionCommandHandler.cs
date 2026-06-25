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

            question.CorrectAnswerTeamId = request.CorrectAnswerTeamId;
            question.IsResolved = true;
            await bonusQuestionRepository.UpdateAsync(question, cancellationToken);

            var tips = await bonusTipRepository.GetByQuestionIdAsync(request.BonusQuestionId, cancellationToken);

            if (BonusQuestionConstants.SemifinalistQuestionIds.Contains(request.BonusQuestionId))
            {
                // Alle bereits aufgelösten korrekten Halbfinale-Teams
                var allSemiQuestions = await bonusQuestionRepository.GetByIdsAsync(
                    BonusQuestionConstants.SemifinalistQuestionIds, cancellationToken);

                var correctTeams = allSemiQuestions
                    .Where(q => q.IsResolved && q.CorrectAnswerTeamId.HasValue)
                    .Select(q => q.CorrectAnswerTeamId!.Value)
                    .ToHashSet();

                // Alle bisherigen Halbfinale-Tips aller User laden (für Duplikat-Check)
                var allSemiTips = await bonusTipRepository.GetByQuestionIdsAsync(
                    BonusQuestionConstants.SemifinalistQuestionIds, cancellationToken);

                foreach (var tip in tips)
                {
                    bool tippedCorrectTeam = correctTeams.Contains(tip.AnswerTeamId);

                    // Hat dieser User dasselbe Team bereits bei einer anderen Halbfinale-Frage Punkte bekommen?
                    bool alreadyRewarded = allSemiTips.Any(t =>
                        t.UserId == tip.UserId &&
                        t.AnswerTeamId == tip.AnswerTeamId &&
                        t.Points > 0 &&
                        t.Id != tip.Id);

                    tip.Points = (tippedCorrectTeam && !alreadyRewarded) ? question.Points : 0;
                    await bonusTipRepository.UpdateAsync(tip, cancellationToken);
                }
            }
            else
            {
                foreach (var tip in tips)
                {
                    tip.Points = tip.AnswerTeamId == request.CorrectAnswerTeamId ? question.Points : 0;
                    await bonusTipRepository.UpdateAsync(tip, cancellationToken);
                }
            }
        }
    }
}
