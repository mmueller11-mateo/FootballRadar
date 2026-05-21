namespace FootballRadar.WebApp.Models
{
    namespace FootballRadar.WebApp.Models
    {
        public class WmTeamStandingRow
        {
            public string Team { get; set; } = "";
            public string Logo { get; set; } = "";

            public int Played { get; set; }
            public int Won { get; set; }
            public int Drawn { get; set; }
            public int Lost { get; set; }

            public int GoalsFor { get; set; }
            public int GoalsAgainst { get; set; }

            public int Points => Won * 3 + Drawn;
            public int GoalDifference => GoalsFor - GoalsAgainst;

            public bool IsPredicted { get; set; } = false;
            public int Position { get; set; }
        }
    }
}
