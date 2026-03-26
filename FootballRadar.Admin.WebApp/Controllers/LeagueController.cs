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
        private readonly IMediator _mediator;

        public LeagueController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> LeaguesList()
        {
            var leagues = await _mediator.Send(new GetLeaguesQuery());
            var countries = await _mediator.Send(new GetCountriesQuery());

            var vm = leagues.Select(l => new LeagueViewModel
            {
                Id = l.Id,
                Name = l.Name,
                CountryName = countries.FirstOrDefault(c => c.Id == l.CountryId)?.Name ?? "Unknown"
            });

            return View(vm);
        }

        public async Task<IActionResult> CreateLeague()
        {
            var countries = await _mediator.Send(new GetCountriesQuery());
            var vm = new CreateLeagueViewModel
            {
                Countries = countries.Select(c => new CountryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeague(CreateLeagueViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var countries = await _mediator.Send(new GetCountriesQuery());
                vm.Countries = countries.Select(c => new CountryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();
                return View(vm);
            }

            await _mediator.Send(new CreateLeagueCommand
            {
                Name = vm.Name,
                CountryId = vm.CountryId,
                ApiLeagueId = vm.ApiLeagueId
            });

            return RedirectToAction(nameof(LeaguesList));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteLeagueCommand { Id = id });
            return RedirectToAction(nameof(LeaguesList));
        }
    }
}