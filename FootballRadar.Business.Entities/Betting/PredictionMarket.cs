using System.ComponentModel.DataAnnotations.Schema;

namespace FootballRadar.Business.Entities.Betting
{
    public abstract class PredictionMarket
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }

        [NotMapped]
        public ICollection<IPredictionMarketRule> Rules { get; set; }
        /// <summary>
        /// Gets or sets the reward.(Total bidding amount * n%)
        /// </summary>
        /// <value>
        /// The reward.
        /// </value>
        public int Reward { get; set; }

    }
}
