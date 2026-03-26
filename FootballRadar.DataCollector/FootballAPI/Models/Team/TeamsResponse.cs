using FootballRadar.DataCollector.FootballAPI.Models.Country;

namespace FootballRadar.DataCollector.FootballAPI.Models.Team
{
    public class TeamResponse
    {
        public TeamInfo Team { get; set; }
        public VenueInfo Venue { get; set; }
        public CountryInfo Country { get; set; }
    }
}
