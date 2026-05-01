using FootballRadar.Business.Entities.Enums;

namespace FootballRadar.Business.Entities.TransferEntities
{
    public class TransferRumor
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid? SourceTeamId { get; set; }
        public Guid TargetTeamId { get; set; }
        public RumorCredibility Credibility { get; set; }
        public required string Source { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public RumorStatus Status { get; set; }
    }

}
