using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ParkKing.ViewModels;
using ParkKing.Data;
using System.Linq;
using ParkKing.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace ParkKing.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICarRepository carRepo;
        private readonly string bayNoSessionKey;

        public HomeController(IOptions<SessionConfiguration> options, ICarRepository carRepo)
        {
            this.carRepo = carRepo;
            bayNoSessionKey = options.Value.BayNoSessionKey;
        }

        public IActionResult Index()
        {
            // calc amount of spaces left
            var spaces = carRepo.BayAmount - carRepo.GetAll().Count();

            // check session if parked
            var isParked = HttpContext.Session.TryGetValue(bayNoSessionKey, out _);

            return View((AvailableSpace: spaces, UserHasCarParked: isParked ));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
