using FootballRadar.Abstractions;
using FootballRadar.Business.Entities.LeagueEntities;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.Data.Repositories
{
    internal class LeagueRepository : ILeagueRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> dbFactory;

        public LeagueRepository(IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public async Task<IReadOnlyCollection<PublicLeague>> GetLeaguesAsync()
        {
            using var db = await dbFactory.CreateDbContextAsync();
            return await db.Leagues
                .Where(l => db.Standings.Any(s => s.LeagueId == l.Id))
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Standing>> GetStandingsAsync(int apiLeagueId, int season)
        {
            using var db = await dbFactory.CreateDbContextAsync();

            var league = await db.Leagues.FirstOrDefaultAsync(l => l.ApiLeagueId == apiLeagueId);
            if (league == null)
                return Array.Empty<Standing>();

            return await db.Standings
                .Where(s => s.LeagueId == league.Id && s.Season == season)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<StandingWithDetails>> GetStandingsWithDetailsAsync(int apiLeagueId, int season)
        {
            using var db = await dbFactory.CreateDbContextAsync();

            var league = await db.Leagues.FirstOrDefaultAsync(l => l.ApiLeagueId == apiLeagueId);
            if (league == null)
                return Array.Empty<StandingWithDetails>();

            return await db.Standings
                .Where(s => s.LeagueId == league.Id && s.Season == season)
                .Select(s => new StandingWithDetails
                {
                    Standing = s,
                    Team = db.Teams.FirstOrDefault(t => t.Id == s.TeamId),
                    Stats = db.StandingStats.FirstOrDefault(st => st.Id == s.StandingStatsId)
                })
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<League>> GetTopLeaguesAsync()
        {
            using var db = await dbFactory.CreateDbContextAsync();

            var topLeagueIds = new int[] {
            39,   // Premier League
            40,   // Championship
            140,  // La Liga
            78,   // Bundesliga
            135,  // Serie A
            61   // Ligue 1
            };

            return await db.Leagues.Where(l => topLeagueIds.Contains(l.ApiLeagueId)).ToListAsync();
        }
    }
}
