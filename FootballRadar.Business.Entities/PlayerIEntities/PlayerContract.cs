namespace FootballRadar.Business.Entities.PlayerIEntities
{
    public class PlayerContract
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid TeamId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        /// <summary>
        /// Gets or sets the release clause.
        /// </summary>
        /// <value>
        /// The release clause.
        /// </value>
        /// <remarks>
        /// Sometimes the amount of release clause is not published publicy
        /// </remarks>
        public Money? ReleaseClause { get; set; }
    }
}