using FootballRadar.DataCollector.FootballAPI.Models.Country;

namespace FootballRadar.DataCollector.FootballAPI.Models.League
{
    public class LeagueResponse
    {
        public LeagueInfo League { get; set; }
        public CountryInfo Country { get; set; }
    }

}
