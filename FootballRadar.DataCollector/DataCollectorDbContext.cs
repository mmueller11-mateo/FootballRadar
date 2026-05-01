using FootballRadar.Business.Entities;
using FootballRadar.Business.Entities.LeagueEntities;
using FootballRadar.Business.Entities.ManagerEntities;
using FootballRadar.Business.Entities.PlayerIEntities;
using FootballRadar.Business.Entities.TeamEntities;
using FootballRadar.Business.Entities.TransferEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace FootballRadar.DataCollector.ApiSports
{
    public class DataCollectorDbContext : DbContext
    {
        public DataCollectorDbContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<PublicLeague> Leagues { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<ManagerAssignment> ManagerAssignments { get; set; }
        public DbSet<ManagerStatistics> ManagerStatistics { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<NationalTeamManagerAssignment> NationalTeamManagerAssignments { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerContract> PlayerContracts { get; set; }
        public DbSet<PlayerInjury> PlayerInjuries { get; set; }
        public DbSet<PlayerMarketValue> PlayerMarketValues { get; set; }
        public DbSet<PlayerPosition> PlayerPositions { get; set; }
        public DbSet<PlayerStatistics> PlayerStatistics { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<TransferRumor> TransferRumors { get; set; }
        public DbSet<TransferWindow> TransferWindows { get; set; }
        public DbSet<Trophy> Trophies { get; set; }
        public DbSet<Standing> Standings { get; set; }
        public DbSet<StandingStats> StandingStats { get; set; }
        public DbSet<Match> Fixtures { get; set; }
        public DbSet<TeamSeasonPlayer> TeamSeasonPlayers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerContract>(entity =>
            {
                entity.Property(e => e.ReleaseClause)
                    .HasConversion(
                        v => v.HasValue ? JsonSerializer.Serialize(v) : null,
                        v => v != null ? JsonSerializer.Deserialize<Money>(v) : (Money?)null);
            });

            modelBuilder.Entity<PlayerMarketValue>(entity =>
            {
                entity.Property(e => e.Value)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v),
                        v => JsonSerializer.Deserialize<Money>(v));
            });

            modelBuilder.Entity<Transfer>(entity =>
            {
                entity.Property(e => e.Fee)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v),
                        v => JsonSerializer.Deserialize<Money>(v));
            });

            modelBuilder.Entity<PublicLeague>(entity =>
            {
                entity.HasKey(l => l.Id);
                entity.Property(l => l.Name).IsRequired().HasMaxLength(100);
                entity.Property(l => l.Logo).HasMaxLength(2048);
            });

            modelBuilder.Entity<PublicLeague>(entity =>
            {
                entity.HasKey(l => l.Id);
                entity.Property(l => l.Name).IsRequired().HasMaxLength(100);
                entity.Property(l => l.Logo).HasMaxLength(2048);
            });

            modelBuilder.Entity<Match>(entity =>
            {
                entity.ToTable("Fixtures");
                entity.HasKey(l => l.Id);
            });
        }
    }
}
