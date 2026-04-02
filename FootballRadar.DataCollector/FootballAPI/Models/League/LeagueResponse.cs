using FootballRadar.DataCollector.ApiSports.FootballAPI.Models.Country;

namespace FootballRadar.DataCollector.ApiSports.FootballAPI.Models.League
{
    public class LeagueResponse
    {
        public required LeagueInfo League { get; set; }
        public required CountryInfo Country { get; set; }
    }

}
