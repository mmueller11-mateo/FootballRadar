using FootballRadar.Admin.Business.Services.Commands.Create;
using FootballRadar.Admin.Business.Services.Commands.Delete;
using FootballRadar.Admin.Business.Services.Queries;
using FootballRadar.Admin.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.Admin.WebApp.Controllers
{
    public class CountryController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> CountriesList(CancellationToken cancellationToken = default)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();
            var countries = await mediator.Send(new GetCountriesQuery(), cancellationToken);
            var vm = countries.Select(c => new CountryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code,
                Flag = c.Flag,
            });
            return View(vm);
        }

        [HttpGet]
        public IActionResult CreateCountry()
        {
            return View(new CreateCountryViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateCountry(CreateCountryViewModel vm, CancellationToken cancellationToken = default)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();

            if (!ModelState.IsValid)
                return View(vm);

            await mediator.Send(new CreateCountryCommand
            {
                Name = vm.Name,
                Code = vm.Code,
                Flag = vm.Flag
            }, cancellationToken);

            return RedirectToAction(nameof(CountriesList));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var mediator = HttpContext.RequestServices.GetRequiredService<IMediator>();
            await mediator.Send(new DeleteCountryCommand { Id = id }, cancellationToken);
            return RedirectToAction(nameof(CountriesList));
        }
    }
}