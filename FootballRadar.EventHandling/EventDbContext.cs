using Microsoft.EntityFrameworkCore;
using My.Framework.EventHandling;

namespace FootballRadar.EventHandling
{
    sealed class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions<EventDbContext> options)
            : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<EventRecord> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<EventRecord>(entity =>
            {
                entity.ToTable("Events");
                entity.HasKey(e => e.Id);
            });

        }
    }
}
