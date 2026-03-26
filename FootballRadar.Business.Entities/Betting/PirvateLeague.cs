using FootballRadar.Business.Entities.LeagueEntities;

namespace FootballRadar.Business.Entities.Betting
{
    public class PrivateLeague : League
    {
        public Guid OwnerUserId { get; set; }
    }
}
