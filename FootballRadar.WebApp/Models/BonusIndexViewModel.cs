using FootballRadar.Business.Entities.TeamEntities;
using FootballRadar.Business.Services.Queries;

namespace FootballRadar.WebApp.Models
{
    public class BonusIndexViewModel
    {
        public List<BonusQuestionWithTip> QuestionsWithTips { get; set; } = [];
        public List<NationalTeam> Teams { get; set; } = [];
    }
}
