namespace FootballRadar.Business.Entities.ManagerEntities
{
    public class ManagerAssignment
    {
        public Guid Id { get; set; }
        public Guid ManagerId { get; set; }
        public Guid TeamId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
