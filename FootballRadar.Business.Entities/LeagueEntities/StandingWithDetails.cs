using FootballRadar.Business.Entities.TeamEntities;

namespace FootballRadar.Business.Entities.LeagueEntities
{
    public class StandingWithDetails
    {
        public Standing Standing { get; set; }
        public Team? Team { get; set; }
        public StandingStats? Stats { get; set; }
    }
}
