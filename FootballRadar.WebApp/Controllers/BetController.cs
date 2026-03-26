//using FootballRadar.Business.Entities.Betting;
//using FootballRadar.WebApp.Models;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;

//namespace FootballRadar.WebApp.Controllers
//{
//    public class BetController : Controller
//    {

//        public BetController()
//        {
//        }

//        public async Task<IActionResult> UpcomingMatches()
//        {
//            return new EmptyResult();
//        }

//        public async Task<IActionResult> PlaceBet(PlaceBetModel placeBetModel)
//        {
//            var api = HttpContext.RequestServices.GetRequiredService<IMediator>();

//            var command = new PlaceBetCommand
//            {
//            };

//            await api.Send(request: command, HttpContext.RequestAborted);
//        }
//    }
//}
