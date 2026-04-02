using FootballRadar.DataCollector.Kaggle.FootballRadar.DataCollector.Kaggle;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FootballRadar.DataCollector.Kaggle
{
    internal class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IConfiguration config;
        private readonly IDbContextFactory<DataCollectorDbContext> dbContextFactory;

        public Worker(ILogger<Worker> logger, IConfiguration config, IDbContextFactory<DataCollectorDbContext> dbContextFactory)
        {
            this.logger = logger;
            this.config = config;
            this.dbContextFactory = dbContextFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await TryDoWork(stoppingToken);

            using PeriodicTimer dailyTimer = new(TimeSpan.FromHours(24));
            try
            {
                while (await dailyTimer.WaitForNextTickAsync(stoppingToken))
                {
                    await TryDoWork(stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Timed Hosted Service is stopping.");
            }
        }

        private async Task TryDoWork(CancellationToken cancellationToken)
        {
            try
            {
                await DoWork(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "DoWork failed.");
            }
        }

        private async Task DoWork(CancellationToken cancellationToken)
        {
            string apiToken = config["Kaggle:ApiToken"]
                ?? throw new InvalidOperationException("Kaggle:ApiToken ist nicht konfiguriert.");
            string outputPath = config["Kaggle:OutputPath"]
                ?? throw new InvalidOperationException("Kaggle:OutputPath ist nicht konfiguriert.");

            const string datasetSlug = "davidcariboo/player-scores";
            const string csvFileName = "transfers.csv";

            KaggleDownloader.DownloadDataset(datasetSlug, outputPath, apiToken);

            var csvPath = Path.Combine(outputPath, csvFileName);
            var extRecords = CsvImporter.Import(csvPath);

            logger.LogInformation("Imported {Count} transfer records.", extRecords.Count);

            await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var players = await db.Players
                .ToDictionaryAsync(p => NameNormalizer.Normalize(p.Name), p => p, cancellationToken);

            var teams = await db.Teams
                .ToDictionaryAsync(t => NameNormalizer.Normalize(t.Name), t => t, cancellationToken);

            int matched = 0;
            int skipped = 0;

            foreach (var record in extRecords)
            {
                var playerKey = NameNormalizer.Normalize(record.PlayerName);
                var fromTeamKey = NameNormalizer.Normalize(record.FromClubName);
                var toTeamKey = NameNormalizer.Normalize(record.ToClubName);

                if (!players.TryGetValue(playerKey, out var player))
                {
                    skipped++;
                    continue;
                }

                if (!teams.TryGetValue(fromTeamKey, out var fromTeam) ||
                    !teams.TryGetValue(toTeamKey, out var toTeam))
                {
                    skipped++;
                    continue;
                }

                logger.LogInformation(
                    "MATCH: {Player} | {FromTeam} -> {ToTeam} | Fee: {Fee} | Date: {Date}",
                    player.Name,
                    fromTeam.Name,
                    toTeam.Name,
                    record.TransferFee,
                    record.TransferDate.ToShortDateString());

                matched++;
            }

            logger.LogInformation("Ergebnis: {Matched} gematcht, {Skipped} übersprungen.", matched, skipped);
        }
    }
}