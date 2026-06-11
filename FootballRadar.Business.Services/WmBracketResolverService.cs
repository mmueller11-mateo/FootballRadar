using FootballRadar.Business.Entities.TeamEntities;
using FootballRadar.Business.Entities.TippSpiel;

namespace FootballRadar.Business.Services
{
    public class WmBracketResolverService
    {
        public NationalTeam? Resolve(
    string qualificationCode,
    IReadOnlyDictionary<string, List<GroupStandingEntry>> groupStandings,
    IEnumerable<NationalTeam> allTeams,
    HashSet<Guid> usedTeamIds)
        {
            var position = int.Parse(qualificationCode[..1]);
            var groupPart = qualificationCode[1..];

            if (!groupPart.Contains('/'))
            {
                if (!groupStandings.TryGetValue(groupPart, out var standing))
                    return null;

                var entry = standing.ElementAtOrDefault(position - 1);
                return entry == null ? null : allTeams.FirstOrDefault(t => t.Id == entry.TeamId);
            }
            else
            {
                var groups = groupPart.Split('/');

                var bestThird = groups
                    .Where(g => groupStandings.ContainsKey(g))
                    .Select(g => groupStandings[g].ElementAtOrDefault(2))
                    .Where(e => e != null)
                    .Where(e => !usedTeamIds.Contains(e!.TeamId))
                    .OrderByDescending(e => e!.Points)
                    .ThenByDescending(e => e!.GoalDifference)
                    .ThenByDescending(e => e!.GoalsFor)
                    .FirstOrDefault();

                if (bestThird == null)
                    return null;

                usedTeamIds.Add(bestThird.TeamId);

                return allTeams.FirstOrDefault(t => t.Id == bestThird.TeamId);
            }
        }
    }
}