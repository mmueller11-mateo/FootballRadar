using FootballRadar.Business.Entities;
using FootballRadar.Business.Entities.Betting;
using FootballRadar.Business.Entities.LeagueEntities;
using FootballRadar.Business.Entities.PlayerIEntities;
using FootballRadar.Business.Entities.TeamEntities;
using FootballRadar.Business.Entities.TippSpiel;
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
            ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<PublicLeague> Leagues { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Standing> Standings { get; set; }
        public DbSet<StandingStats> StandingStats { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<WinnerBet> MatchBets { get; set; }
        public DbSet<TransferBet> TransferBets { get; set; }
        public DbSet<PredictionMarket> PredictionMarkets { get; set; }
        public DbSet<MatchPredictionMarket> MatchPredictionMarkets { get; set; }
        public DbSet<TransferPredictionMarket> TransferPredictionMarkets { get; set; }
        public DbSet<Match> Fixtures { get; set; }
        public DbSet<TransferRumor> TransferRumors { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<TeamSeasonPlayer> TeamSeasonPlayers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<WmTip> WmTips { get; set; }
        public DbSet<NationalTeam> NationalTeams { get; set; }
        public DbSet<BonusQuestion> BonusQuestions { get; set; }
        public DbSet<BonusTip> BonusTips { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserProfile>(entity =>
            {
                entity.ToTable("UserProfiles");
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.EventNotificationPreferences)
                    .HasConversion(
                        property => JsonSerializer.Serialize(property),
                        value => JsonSerializer.Deserialize<Dictionary<string, bool>>(value) ?? new Dictionary<string, bool>()
                    );
                entity.HasOne<User>()
                    .WithOne()
                    .HasForeignKey<UserProfile>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Bet>(entity =>
            {
                entity.ToTable("Bets");
                entity.HasKey(e => e.Id);
                entity.HasDiscriminator<string>("Discriminator")
                    .HasValue<Bet>("Bet")
                    .HasValue<WinnerBet>("MatchBet")
                    .HasValue<TransferBet>("TransferBet");
            });

            builder.Entity<WinnerBet>(entity =>
            {
                entity.Property(e => e.Prediction)
                  .HasConversion<string>()
                  .HasColumnName("Prediction");
            });

            builder.Entity<TransferBet>(entity =>
            {
                entity.Property(e => e.Prediction)
                       .HasConversion<string>()
                       .HasColumnName("Prediction");
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

            builder.Entity<PredictionMarket>(entity =>
            {
                entity.ToTable("PredictionMarkets");
                entity.HasKey(e => e.Id);
                entity.HasDiscriminator<string>("MarketType")
                    .HasValue<PredictionMarket>("PredictionMarket")
                    .HasValue<MatchPredictionMarket>("MatchPredictionMarket")
                    .HasValue<TransferPredictionMarket>("TransferPredictionMarket");
            });

            builder.Entity<WmTip>(entity =>
            {
                entity.ToTable("WmTips");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.UserId, e.WmMatchId }).IsUnique();
            });

            builder.Entity<Match>(entity =>
            {
                entity.ToTable("Fixtures");
                entity.Property(e => e.WmPhase).HasConversion<string>();
            });

            builder.Entity<NationalTeam>(entity =>
            {
                entity.ToTable("NationalTeams");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Level).HasConversion<int>();
            });
        }
    }
}
