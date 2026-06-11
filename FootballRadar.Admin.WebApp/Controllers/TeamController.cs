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
        [HttpGet]
        public async Task<IActionResult> TeamsList(CancellationToken cancellationToken = default)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();
            var teams = await mediator.Send(new GetTeamsQuery(), cancellationToken);
            var countries = await mediator.Send(new GetCountriesQuery(), cancellationToken);
            var vm = teams.Select(t => new TeamViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Logo = t.Logo,
                Code = t.Code,
                CountryFlag = countries.FirstOrDefault(c => c.Id == t.CountryId)!.Flag
            });
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> CreateTeam(CancellationToken cancellationToken = default)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();
            var countries = await mediator.Send(new GetCountriesQuery(), cancellationToken);
            var vm = new CreateTeamViewModel
            {

                Countries = countries.Select(c => new CountryOption
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam(CreateTeamViewModel vm, CancellationToken cancellationToken = default)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();
            if (!ModelState.IsValid)
            {
                var countries = await mediator.Send(new GetCountriesQuery(), cancellationToken);
                vm.Countries = countries.Select(c => new CountryOption
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();
                return View(vm);
            }

            await mediator.Send(new CreateTeamCommand
            {
                Name = vm.Name,
                CountryId = vm.CountryId,
                ApiTeamId = vm.ApiTeamId,
                Logo = vm.Logo,
                Code = vm.Code
            }, cancellationToken);
            return RedirectToAction(nameof(TeamsList));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();
            await mediator.Send(new DeleteTeamCommand { Id = id }, cancellationToken);
            return RedirectToAction(nameof(TeamsList));
        }
    }
}