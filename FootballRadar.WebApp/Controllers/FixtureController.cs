using FootballRadar.Business.Entities.LeagueEntities;
using FootballRadar.Business.Services.Queries;
using FootballRadar.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.WebApp.Controllers
{

    namespace FootballRadar.WebApp.Controllers
    {
        public class FixtureController : Controller
        {
            private readonly IMediator _mediator;

            public FixtureController(IMediator mediator)
            {
                _mediator = mediator;
            }

            public async Task<IActionResult> Fixtures(int leagueId, int? season = null, CancellationToken cancellationToken = default)
            {
                var seasons = await _mediator.Send(new GetSeasonsByLeagueQuery { ApiLeagueId = leagueId }, cancellationToken);
                var selectedSeason = season ?? seasons.FirstOrDefault();

                var fixtures = await _mediator.Send(new GetMatchesBySeasonQuery { ApiLeagueId = leagueId, Season = selectedSeason }, cancellationToken);
                var teams = await _mediator.Send(new GetAllTeamsQuery(), cancellationToken);
                var leagues = await _mediator.Send(new GetLeaguesQuery(), cancellationToken);
                var league = leagues.FirstOrDefault(l => l.ApiLeagueId == leagueId);
                var now = DateTime.UtcNow;

                FixtureViewModel MapFixture(Match f)
                {
                    var home = teams.FirstOrDefault(t => t.Id == f.HomeTeamId);
                    var away = teams.FirstOrDefault(t => t.Id == f.AwayTeamId);
                    return new FixtureViewModel
                    {
                        Id = f.Id,
                        Date = f.Date,
                        Round = f.Round ?? string.Empty,
                        Status = f.Status ?? string.Empty,
                        HomeTeam = home?.Name ?? "Unknown",
                        AwayTeam = away?.Name ?? "Unknown",
                        HomeLogo = home?.Logo,
                        AwayLogo = away?.Logo,
                        HomeGoals = f.HomeGoals,
                        AwayGoals = f.AwayGoals
                    };
                }

                var vm = new FixturesViewModel
                {
                    LeagueId = leagueId,
                    LeagueName = league!.Name ?? "Unknown",
                    LeagueLogo = league.Logo,
                    Seasons = seasons,
                    SelectedSeason = selectedSeason,
                    UpcomingFixtures = fixtures.Where(f => f.Date > now).Select(MapFixture).ToList(),
                    PastFixtures = fixtures.Where(f => f.Date <= now).OrderByDescending(f => f.Date).Select(MapFixture).ToList()
                };

                return View(vm);
            }
        }
    }
}
