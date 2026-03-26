using FootballRadar.Business.Entities;
using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Entities.LeagueEntities;
using FootballRadar.Business.Entities.TeamEntities;
using FootballRadar.Business.Entities.TransferEntities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FootballRadar.Data
{
    sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<PublicLeague> Leagues { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Standing> Standings { get; set; }
        public DbSet<StandingStats> StandingStats { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<PredictionMarket> PredictionMarkets { get; set; }
        public DbSet<MatchPredictionMarket> MatchPredictionMarkets { get; set; }
        public DbSet<TransferPredictionMarket> TransferPredictionMarkets { get; set; }
        public DbSet<Match> Fixtures { get; set; }
        public DbSet<TransferRumor> TransferRumors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Bet>(entity =>
            {
                entity.ToTable("Bets");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasConversion(x => JsonSerializer.Serialize(x), x => JsonSerializer.Deserialize<Money>(x));
            });

            builder.Entity<Wallet>(entity =>
            {
                entity.ToTable("Wallets");
                entity.HasKey(e => e.Id);
            });

            builder.Entity<WalletTransaction>(entity =>
            {
                entity.ToTable("WalletTransactions");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasConversion(x => JsonSerializer.Serialize(x), x => JsonSerializer.Deserialize<Money>(x));
                entity.Property(e => e.Type).HasConversion<string>();
                entity.Property(e => e.Status).HasConversion<string>();

            });
        }
    }
}
