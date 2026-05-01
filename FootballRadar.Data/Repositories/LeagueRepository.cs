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

        public async Task<IEnumerable<PublicLeague>> GetLeaguesAsync(CancellationToken cancellationToken = default)
        {
            using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
            return await db.Leagues
                .Where(l => db.Standings.Any(s => s.LeagueId == l.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Standing>> GetStandingsAsync(int apiLeagueId, int season, CancellationToken cancellationToken = default)
        {
            using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

            var league = await db.Leagues.FirstOrDefaultAsync(l => l.ApiLeagueId == apiLeagueId, cancellationToken);
            if (league == null)
                return Array.Empty<Standing>();

            return await db.Standings
                .Where(s => s.LeagueId == league.Id && s.Season == season)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<StandingWithDetails>> GetStandingsWithDetailsAsync(int apiLeagueId, int season, CancellationToken cancellationToken = default)
        {
            using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

            var league = await db.Leagues.FirstOrDefaultAsync(l => l.ApiLeagueId == apiLeagueId, cancellationToken);
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
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<League>> GetTopLeaguesAsync(CancellationToken cancellationToken = default)
        {
            using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

            var topLeagueIds = new int[] {
            39,   // Premier League
            40,   // Championship
            140,  // La Liga
            78,   // Bundesliga
            135,  // Serie A
            61   // Ligue 1
            };

            return await db.Leagues.Where(l => topLeagueIds.Contains(l.ApiLeagueId)).ToListAsync(cancellationToken);
        }
    }
}
