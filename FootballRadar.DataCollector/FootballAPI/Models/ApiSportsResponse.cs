namespace FootballRadar.DataCollector.FootballAPI.Models
{
    public class ApiSportsResponse<T>
    {
        public IReadOnlyCollection<T> Response { get; set; }
    }
}
