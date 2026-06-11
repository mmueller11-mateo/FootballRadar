using FootballRadar.Business.Entities;
using FootballRadar.Business.Entities.LeagueEntities;
using FootballRadar.Business.Entities.TeamEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FootballRadar.Admin.Data.ORM
{
    sealed class AdminDbContext : DbContext
    {
        public AdminDbContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<PublicLeague> Leagues { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<NationalTeam> NationalTeams { get; set; }
        public DbSet<Match> Fixtures { get; set; }
        public DbSet<WmTip> WmTips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Countries", t => t.ExcludeFromMigrations());
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Code);
                entity.Property(c => c.Flag);
            });

            modelBuilder.Entity<PublicLeague>(entity =>
            {
                entity.ToTable("Leagues", t => t.ExcludeFromMigrations());
                entity.HasKey(l => l.Id);
                entity.Property(l => l.Name).IsRequired().HasMaxLength(100);
                entity.Property(l => l.Logo).HasMaxLength(2048);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("Teams", t => t.ExcludeFromMigrations());
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<NationalTeam>(entity =>
            {
                entity.HasKey(nt => nt.Id);
                entity.Property(nt => nt.Name).IsRequired().HasMaxLength(100);
                entity.Property(nt => nt.Level).IsRequired();
                entity.Property(nt => nt.CountryId).IsRequired();
            });
            modelBuilder.Entity<Match>(entity =>
            {
                entity.ToTable("Fixtures", t => t.ExcludeFromMigrations());
                entity.HasKey(m => m.Id);
                entity.Property(m => m.WmPhase).HasConversion<string>();
            });

            modelBuilder.Entity<WmTip>(entity =>
            {
                entity.ToTable("WmTips", t => t.ExcludeFromMigrations());
                entity.HasKey(t => t.Id);
            });
        }
    }
}