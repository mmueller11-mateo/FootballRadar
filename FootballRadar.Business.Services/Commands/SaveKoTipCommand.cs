using MediatR;

namespace FootballRadar.Business.Services.Commands
{
    public sealed class SaveKoTipCommand : IRequest
    {
        public Guid UserId { get; set; }
        public Guid FixtureId { get; set; }
        public Guid? WinnerId { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
    }
}
