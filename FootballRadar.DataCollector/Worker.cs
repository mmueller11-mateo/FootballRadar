using FootballRadar.Business.Entities;
using FootballRadar.Business.Entities.LeagueEntities;
using FootballRadar.Business.Entities.TeamEntities;
using FootballRadar.DataCollector.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace FootballRadar.DataCollector
{
    internal class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ILeagueService _leagueService;
        private readonly ICountryService _countryService;
        private readonly ITeamService _teamService;
        private readonly IFixtureService _fixtureService;
        private readonly IDbContextFactory<DataCollectorDbContext> _dbContextFactory;

        public Worker(ILogger<Worker> logger, ILeagueService leagueService, IDbContextFactory<DataCollectorDbContext> dbContextFactory, ICountryService countryService, ITeamService teamService, IFixtureService fictureService)
        {
            _logger = logger;
            _leagueService = leagueService;
            _dbContextFactory = dbContextFactory;
            _countryService = countryService;
            _teamService = teamService;
            _fixtureService = fictureService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await DoWork(stoppingToken);
            using PeriodicTimer dailyTimer = new(TimeSpan.FromHours(24));
            try
            {
                while (await dailyTimer.WaitForNextTickAsync(stoppingToken))
                {
                    await DoWork(stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Timed Hosted Service is stopping.");
            }
        }

        private async Task DoWork(CancellationToken cancellationToken)
        {
            //    await GetCountriesJob(cancellationToken);
            //    await GetLeaguesJob(cancellationToken);
            //    await GetTeamsJob(cancellationToken);
            //    await GetStandingsJob(cancellationToken);
            await GetFixturesJob(cancellationToken);
        }

        private async Task GetLeaguesJob(CancellationToken cancellationToken)
        {
            var leagues = await _leagueService.GetLeaguesAsync();
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            foreach (var league in leagues)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                bool alreadyExists = await dbContext.Leagues.AnyAsync(l => l.ApiLeagueId == league.League.Id);

                if (alreadyExists)
                {
                    continue;
                }

                var country = await dbContext.Countries.Where(c => c.Code == league.Country.Code).FirstOrDefaultAsync();

                dbContext.Leagues.Add(new PublicLeague
                {
                    Id = Guid.NewGuid(),
                    ApiLeagueId = league.League.Id,
                    Name = league.League.Name,
                    CountryId = country?.Id,
                    Logo = league.League.Logo
                });
            }

            await dbContext.SaveChangesAsync();
        }

        private async Task GetCountriesJob(CancellationToken cancellationToken)
        {
            var countries = await _countryService.GetCountriesAsync();
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            foreach (var country in countries)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                bool alreadyExists = await dbContext.Countries.AnyAsync(c => c.Code == country.Code);
                if (alreadyExists)
                {
                    continue;
                }
                dbContext.Countries.Add(new Country
                {
                    Id = Guid.NewGuid(),
                    Code = country.Code,
                    Name = country.Name,
                    Flag = country.Flag
                });
            }
            await dbContext.SaveChangesAsync();
        }

        private static readonly int[] TopLeagueIds = {
            39,   // Premier League
            40,   // Championship
            //140,  // La Liga
            //78,   // Bundesliga
            //135,  // Serie A
            //61,   // Ligue 1
        };

        private async Task GetTeamsJob(CancellationToken cancellationToken)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            foreach (var leagueId in TopLeagueIds)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                var league = await dbContext.Leagues.FirstOrDefaultAsync(l => l.ApiLeagueId == leagueId);
                var country = league?.CountryId != null ? await dbContext.Countries.FirstOrDefaultAsync(c => c.Id == league.CountryId) : null;
                var teams = await _teamService.GetTeamsAsync(leagueId, 2023);

                foreach (var team in teams)
                {
                    bool alreadyExists = await dbContext.Teams.AnyAsync(t => t.ApiTeamId == team.Team.Id);
                    if (alreadyExists)
                        continue;

                    dbContext.Teams.Add(new Team
                    {
                        Id = Guid.NewGuid(),
                        ApiTeamId = team.Team.Id,
                        Name = team.Team.Name,
                        Code = team.Team.Code,
                        Logo = team.Team.Logo,
                        CountryId = country?.Id
                    });
                }
                await dbContext.SaveChangesAsync();
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
        private async Task GetStandingsJob(CancellationToken cancellationToken)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            foreach (var leagueId in TopLeagueIds)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                try
                {
                    var standings = await _leagueService.GetStandingsAsync(leagueId, 2024);

                    var league = await dbContext.Leagues.FirstOrDefaultAsync(l => l.ApiLeagueId == leagueId);
                    if (league == null)
                    {
                        _logger.LogWarning("Liga nicht gefunden für ApiLeagueId: {LeagueId}", leagueId);
                        continue;
                    }

                    var existing = dbContext.Standings.Where(s => s.LeagueId == league.Id && s.Season == 2024);
                    dbContext.Standings.RemoveRange(existing);

                    foreach (var standing in standings)
                    {
                        var team = await dbContext.Teams.FirstOrDefaultAsync(t => t.ApiTeamId == standing.Team.Id);
                        if (team == null)
                        {
                            _logger.LogWarning("Team nicht gefunden für ApiTeamId: {TeamId}", standing.Team.Id);
                            continue;
                        }

                        var stats = new StandingStats
                        {
                            Id = Guid.NewGuid(),
                            Played = standing.All.Played,
                            Win = standing.All.Win,
                            Draw = standing.All.Draw,
                            Lose = standing.All.Lose
                        };
                        dbContext.StandingStats.Add(stats);

                        dbContext.Standings.Add(new Standing
                        {
                            Id = Guid.NewGuid(),
                            Season = 2024,
                            Rank = standing.Rank,
                            TeamId = team.Id,
                            LeagueId = league.Id,
                            Points = standing.Points,
                            GoalsDiff = standing.GoalsDiff,
                            StandingStatsId = stats.Id,
                        });
                    }

                    await dbContext.SaveChangesAsync();
                    await Task.Delay(TimeSpan.FromSeconds(2));
                }
                catch (Refit.ApiException ex) when ((int)ex.StatusCode == 429)
                {
                    _logger.LogWarning("Rate limit erreicht, warte 60 Sekunden...");
                    await Task.Delay(TimeSpan.FromSeconds(60));
                }
            }
        }

        private async Task GetFixturesJob(CancellationToken cancellationToken)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            foreach (var leagueId in TopLeagueIds)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    var fixtures = await _fixtureService.GetFixturesAsync(leagueId, 2024);

                    var league = await dbContext.Leagues.FirstOrDefaultAsync(l => l.ApiLeagueId == leagueId);
                    if (league == null)
                    {
                        _logger.LogWarning("Liga nicht gefunden für ApiLeagueId: {LeagueId}", leagueId);
                        continue;
                    }

                    foreach (var fixture in fixtures)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        var homeTeam = await dbContext.Teams.FirstOrDefaultAsync(t => t.ApiTeamId == fixture.Teams.Home.Id);
                        var awayTeam = await dbContext.Teams.FirstOrDefaultAsync(t => t.ApiTeamId == fixture.Teams.Away.Id);

                        if (homeTeam == null || awayTeam == null)
                        {
                            _logger.LogWarning("Team nicht gefunden für Fixture {FixtureId}", fixture.Fixture.Id);
                            continue;
                        }

                        var existing = await dbContext.Fixtures.FirstOrDefaultAsync(f => f.ApiFixtureId == fixture.Fixture.Id);
                        if (existing != null)
                        {
                            existing.HomeGoals = fixture.Goals.Home;
                            existing.AwayGoals = fixture.Goals.Away;
                            existing.Status = fixture.Fixture.Status.Short;
                        }
                        else
                        {
                            dbContext.Fixtures.Add(new Match
                            {
                                Id = Guid.NewGuid(),
                                ApiFixtureId = fixture.Fixture.Id,
                                Date = fixture.Fixture.Date,
                                Season = 2024,
                                Round = fixture.League.Round,
                                Status = fixture.Fixture.Status.Short,
                                LeagueId = league.Id,
                                HomeTeamId = homeTeam.Id,
                                AwayTeamId = awayTeam.Id,
                                HomeGoals = fixture.Goals.Home,
                                AwayGoals = fixture.Goals.Home
                            });
                        }
                    }

                    await dbContext.SaveChangesAsync();
                    await Task.Delay(TimeSpan.FromSeconds(2));
                }
                catch (Refit.ApiException ex) when ((int)ex.StatusCode == 429)
                {
                    _logger.LogWarning("Rate limit erreicht, warte 60 Sekunden...");
                    await Task.Delay(TimeSpan.FromSeconds(60));
                }
            }
        }
    }
}