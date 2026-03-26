using FootballRadar.Admin.Business.Services.Commands.Create;
using FootballRadar.Admin.Business.Services.Commands.Delete;
using FootballRadar.Admin.Business.Services.Queries;
using FootballRadar.Admin.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.Admin.WebApp.Controllers
{
    public class TeamController : Controller
    {
        private readonly IMediator _mediator;

        public TeamController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> TeamsList()
        {
            var teams = await _mediator.Send(new GetTeamsQuery());
            var vm = teams.Select(t => new TeamViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Logo = t.Logo,
            });
            return View(vm);
        }

        public async Task<IActionResult> CreateTeam()
        {
            var countries = await _mediator.Send(new GetCountriesQuery());
            var vm = new CreateTeamViewModel
            {
                Countries = countries.Select(c => new CountryOption
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam(CreateTeamViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var countries = await _mediator.Send(new GetCountriesQuery());
                vm.Countries = countries.Select(c => new CountryOption
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();
                return View(vm);
            }

            await _mediator.Send(new CreateTeamCommand
            {
                Name = vm.Name,
                CountryId = vm.CountryId,
                ApiTeamId = vm.ApiTeamId,
                Logo = vm.Logo
            });
            return RedirectToAction(nameof(TeamsList));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteTeamCommand { Id = id });
            return RedirectToAction(nameof(TeamsList));
        }
    }
}