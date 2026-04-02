using FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Country;

namespace FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Team
{
    public class TeamResponse
    {
        public required TeamInfo Team { get; set; }
        public required VenueInfo Venue { get; set; }
        public required CountryInfo Country { get; set; }
    }
}
