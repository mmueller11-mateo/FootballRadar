using FootballRadar.Abstractions;
using FootballRadar.Business.Services.Background;
using FootballRadar.Business.Services.MatchPredictionMarketRules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace FootballRadar.Business.Services
{
    public static class ComponentConfigurator
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddTransient<IMatchPredictionRewardCalculator, MatchPredictionRewardCalculator>();
            services.AddTransient<ITransferPredictionRewardCalculator, TransferPredictionRewardCalculator>();
            services.AddTransient<IBetRuleFactory, BetRuleFactory>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<ICurrencyConverter, CurrencyConverter>();
            services.AddHostedService<WalletTransactionExecuter>();
            services.AddHostedService<BetSettlementWorker>();
        }
    }
}
