using FootballRadar.Business.Entities.TippSpiel;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.TippSpiel.Data
{
    public class TippspielDbContext : DbContext
    {
        public TippspielDbContext(DbContextOptions<TippspielDbContext> options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<TippMatch> TippMatches { get; set; }
        public DbSet<Tipper> Tippers { get; set; }
        public DbSet<Tip> Tips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TippMatch>(entity =>
            {
                entity.ToTable("TippMatches");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.HomeTeam).IsRequired().HasMaxLength(100);
                entity.Property(e => e.AwayTeam).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phase).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<Tipper>(entity =>
            {
                entity.ToTable("Tippers");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            modelBuilder.Entity<Tip>(entity =>
            {
                entity.ToTable("Tips");
                entity.HasKey(e => e.Id);
                entity.HasIndex(t => new { t.TipperId, t.MatchId }).IsUnique();
            });
        }
    }
}