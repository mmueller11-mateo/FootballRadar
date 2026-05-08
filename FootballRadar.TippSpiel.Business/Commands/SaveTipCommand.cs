using MediatR;

namespace FootballRadar.TippSpiel.Business.Commands
{
    public class SaveTipCommand : IRequest
    {
        public Guid TipperId { get; set; }
        public Guid MatchId { get; set; }
        public int PredictedHome { get; set; }
        public int PredictedAway { get; set; }
    }
}
