using FootballRadar.Abstractions;
using FootballRadar.Data.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FootballRadar.Data
{
    public static class ComponentConfigurator
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddDbContextFactory<ApplicationDbContext>((sp, options) =>
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder(configuration.GetConnectionString("ApplicationDB"));
                options.UseSqlServer(connectionStringBuilder.ToString(), sqlServer =>
                {
                    sqlServer.EnableRetryOnFailure(maxRetryCount: 3);
                });

                options.EnableSensitiveDataLogging(environment.IsDevelopment());
                options.EnableThreadSafetyChecks(environment.IsDevelopment());
                options.EnableDetailedErrors(environment.IsDevelopment());
            });

            services.AddTransient<IBetRepository, BetRepository>();
            services.AddTransient<ILeagueRepository, LeagueRepository>();
            services.AddTransient<IPredictionMarketRepository, PredictionMarketRepository>();
            services.AddTransient<IMatchRepository, MatchRepository>();
            services.AddTransient<INationalTeamRepository, NationalTeamRepository>();
            services.AddTransient<ITeamRepository, TeamRepository>();
            services.AddTransient<ITransferRepository, TransferRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IWalletRepository, WalletRepository>();
            services.AddTransient<IWalletTransactionRepository, WalletTransactionRepository>();
            services.AddTransient<IPlayerRepository, PlayerRepository>();
            services.AddTransient<IViewModelRepository, ViewModelRepository>();
            services.AddTransient<IWmTipRepository, WmTipRepository>();
        }
    }
}
