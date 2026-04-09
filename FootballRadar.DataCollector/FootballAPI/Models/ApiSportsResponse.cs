namespace FootballRadar.DataCollector.ApiSports.FootballAPI.Models
{
    public class ApiSportsResponse<T>
    {
        public required IReadOnlyCollection<T> Response { get; set; }
        public Paging Paging { get; set; }
    }
}
