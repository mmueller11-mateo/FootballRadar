using System.ComponentModel.DataAnnotations;

namespace FootballRadar.TippSpiel.WebApp.Models
{
    public class TipViewModel
    {
        public Guid MatchId { get; set; }
        public string HomeTeam { get; set; } = string.Empty;
        public string AwayTeam { get; set; } = string.Empty;
        public DateTime KickoffUtc { get; set; }

        [Range(0, 99)]
        public int PredictedHome { get; set; }

        [Range(0, 99)]
        public int PredictedAway { get; set; }

        public bool IsEdit { get; set; }
    }
}
