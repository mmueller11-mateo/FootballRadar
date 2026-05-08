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
        private readonly IMediator mediator;

        public NationalTeamController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IActionResult> NationalTeamsList(CancellationToken cancellationToken = default)
        {
            var nationalTeams = await mediator.Send(new GetNationalTeamsQuery(), cancellationToken);
            var countries = await mediator.Send(new GetCountriesQuery(), cancellationToken);

            var vm = nationalTeams.Select(nt => new NationalTeamViewModel
            {
                Id = nt.Id,
                Name = nt.Name,
                Level = nt.Level.ToString(),
                Country = countries.FirstOrDefault(c => c.Id == nt.CountryId)?.Name ?? "Unknown"
            });

            return View(vm);
        }

        public async Task<IActionResult> CreateNationalTeam(CancellationToken cancellationToken = default)
        {
            var countries = await mediator.Send(new GetCountriesQuery(), cancellationToken);
            var vm = new CreateNationalTeamViewModel
            {
                Countries = countries.ToList(),
                Levels = Enum.GetValues<NationalTeamLevel>().ToList()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNationalTeam(CreateNationalTeamViewModel vm, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                var countries = await mediator.Send(new GetCountriesQuery(), cancellationToken);
                vm.Countries = countries.ToList();
                vm.Levels = Enum.GetValues<NationalTeamLevel>().ToList();
                return View(vm);
            }

            await mediator.Send(new CreateNationalTeamCommand
            {
                Name = vm.Name,
                CountryId = vm.CountryId,
                Level = vm.Level
            }, cancellationToken);

            return RedirectToAction(nameof(NationalTeamsList));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            await mediator.Send(new DeleteNationalTeamCommand { Id = id }, cancellationToken);
            return RedirectToAction(nameof(NationalTeamsList));
        }
    }
}