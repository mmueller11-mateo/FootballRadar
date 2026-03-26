using FootballRadar.Admin.Business.Services.Commands.Create;
using FootballRadar.Admin.Business.Services.Commands.Delete;
using FootballRadar.Admin.Business.Services.Queries;
using FootballRadar.Admin.WebApp.Models;
using FootballRadar.Business.Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.Admin.WebApp.Controllers
{
    public class NationalTeamController : Controller
    {
        private readonly IMediator _mediator;

        public NationalTeamController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> NationalTeamsList()
        {
            var nationalTeams = await _mediator.Send(new GetNationalTeamsQuery());
            var countries = await _mediator.Send(new GetCountriesQuery());

            var vm = nationalTeams.Select(nt => new NationalTeamViewModel
            {
                Id = nt.Id,
                Name = nt.Name,
                Level = nt.Level.ToString(),
                Country = countries.FirstOrDefault(c => c.Id == nt.CountryId)?.Name ?? "Unknown"
            });

            return View(vm);
        }

        public async Task<IActionResult> CreateNationalTeam()
        {
            var countries = await _mediator.Send(new GetCountriesQuery());
            var vm = new CreateNationalTeamViewModel
            {
                Countries = countries.ToList(),
                Levels = Enum.GetValues<NationalTeamLevel>().ToList()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNationalTeam(CreateNationalTeamViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var countries = await _mediator.Send(new GetCountriesQuery());
                vm.Countries = countries.ToList();
                vm.Levels = Enum.GetValues<NationalTeamLevel>().ToList();
                return View(vm);
            }

            await _mediator.Send(new CreateNationalTeamCommand
            {
                Name = vm.Name,
                CountryId = vm.CountryId,
                Level = vm.Level
            });

            return RedirectToAction(nameof(NationalTeamsList));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteNationalTeamCommand { Id = id });
            return RedirectToAction(nameof(NationalTeamsList));
        }
    }
}