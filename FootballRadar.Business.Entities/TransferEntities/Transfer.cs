using FootballRadar.Business.Entities.Enums;

namespace FootballRadar.Business.Entities.TransferEntities
{
    public class Transfer
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid FromTeamId { get; set; }
        public Guid ToTeamId { get; set; }
        public Money Fee { get; set; }
        public bool IsFeeDisclosed { get; set; }
        public TransferStatus Status { get; set; }
        public DateTimeOffset TransferDate { get; set; }
        public Guid TransferWindowId { get; set; }
    }
}
