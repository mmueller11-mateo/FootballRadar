namespace FootballRadar.Business.Services
{
    public static class BonusQuestionConstants
    {
        public static readonly IReadOnlySet<Guid> SemifinalistQuestionIds = new HashSet<Guid>
        {
            Guid.Parse("A9E3E82C-3841-49FB-A163-62ACEBD46978"), // Team 1
            Guid.Parse("A0D98895-66CA-4D33-8AF0-D5714906C895"), // Team 2
            Guid.Parse("FD61FD75-D822-4E89-B794-BAE03557A99F"), // Team 3
            Guid.Parse("E600C023-39E9-4923-BF35-95A2334A8EFD"), // Team 4
        };
    }
}
