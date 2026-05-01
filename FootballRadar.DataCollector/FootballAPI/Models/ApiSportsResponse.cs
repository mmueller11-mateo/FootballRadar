namespace FootballRadar.DataCollector.ApiSports.FootballAPI.Models
{
    public class ApiSportsResponse<T>
    {
        public required List<T> Response { get; set; }
        public required Paging Paging { get; set; }
    }
}
