using FootballRadar.Business.Services.Queries;
using FootballRadar.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.WebApp.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IMediator mediator;
        public PlayerController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IActionResult> PlayerList(int apiTeamId, int season, CancellationToken cancellationToken = default)
        {
            var seasons = await mediator.Send(new GetTeamSeasonsQuery { ApiTeamId = apiTeamId }, cancellationToken);

            if (season == 0 && seasons.Any())
                season = seasons.Max();

            var players = await mediator.Send(new GetTeamPlayersQuery
            {
                ApiTeamId = apiTeamId,
                Season = season
            }, cancellationToken);

            var vm = new TeamPlayersViewModel
            {
                ApiTeamId = apiTeamId,
                Season = season,
                AvailableSeasons = seasons.OrderByDescending(s => s),
                Players = players.Select(p => new PlayerViewModel
                {
                    Name = p.Name,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    BirthDate = p.BirthDate,
                    Photo = p.Photo,
                    Nationality = p.NationalityCountryId
                }).ToList()
            };

            return View(vm);
        }
    }
}