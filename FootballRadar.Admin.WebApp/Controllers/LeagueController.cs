using FootballRadar.Admin.Business.Services.Commands.Create;
using FootballRadar.Admin.Business.Services.Commands.Delete;
using FootballRadar.Admin.Business.Services.Queries;
using FootballRadar.Admin.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.Admin.WebApp.Controllers
{
    public class LeagueController : Controller
    {
        private readonly IMediator mediator;

        public LeagueController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IActionResult> LeaguesList(CancellationToken cancellationToken = default)
        {
            var leagues = await mediator.Send(new GetLeaguesQuery(), cancellationToken);
            var countries = await mediator.Send(new GetCountriesQuery(), cancellationToken);

            var vm = leagues.Select(l => new LeagueViewModel
            {
                Id = l.Id,
                Name = l.Name,
                CountryName = countries.FirstOrDefault(c => c.Id == l.CountryId)?.Name ?? "Unknown",
                Logo = l.Logo
            }).ToList();

            return View(vm);
        }

        public async Task<IActionResult> CreateLeague(CancellationToken cancellationToken = default)
        {
            var countries = await mediator.Send(new GetCountriesQuery(), cancellationToken);
            var vm = new CreateLeagueViewModel
            {
                Countries = countries.Select(c => new CountryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Code = c.Code,
                    Flag = c.Flag
                }).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeague(CreateLeagueViewModel vm, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                var countries = await mediator.Send(new GetCountriesQuery(), cancellationToken);
                vm.Countries = countries.Select(c => new CountryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Code = c.Code,
                    Flag = c.Flag
                }).ToList();
                return View(vm);
            }

            await mediator.Send(new CreateLeagueCommand
            {
                Name = vm.Name,
                CountryId = vm.CountryId,
                ApiLeagueId = vm.ApiLeagueId,
                Logo = vm.Logo
            }, cancellationToken);

            return RedirectToAction(nameof(LeaguesList));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            await mediator.Send(new DeleteLeagueCommand { Id = id }, cancellationToken);
            return RedirectToAction(nameof(LeaguesList));
        }
    }
}