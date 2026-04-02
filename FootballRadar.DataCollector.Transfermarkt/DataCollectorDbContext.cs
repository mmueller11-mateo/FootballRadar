using FootballRadar.Business.Entities;
using FootballRadar.Business.Entities.LeagueEntities;
using FootballRadar.Business.Entities.PlayerIEntities;
using FootballRadar.Business.Entities.TeamEntities;
using FootballRadar.Business.Entities.TransferEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace FootballRadar.DataCollector.Kaggle
{
    public class DataCollectorDbContext : DbContext
    {
        public DataCollectorDbContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }


        public DbSet<Player> Players { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Transfer> Transfers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transfer>(entity =>
            {
                entity.Property(e => e.Fee)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v),
                        v => JsonSerializer.Deserialize<Money>(v));
            });
        }
    }
}
