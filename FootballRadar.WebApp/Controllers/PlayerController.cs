using FootballRadar.Business.Services.Queries;
using FootballRadar.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.WebApp.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IMediator _mediator;

        public PlayerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> PlayerList(int apiTeamId)
        {
            var players = await _mediator.Send(new GetTeamPlayersQuery { ApiTeamId = apiTeamId });

            var vm = new TeamPlayersViewModel
            {
                ApiTeamId = apiTeamId,
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