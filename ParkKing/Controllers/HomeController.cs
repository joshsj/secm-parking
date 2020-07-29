using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ParkKing.ViewModels;
using ParkKing.Data;
using System.Linq;

namespace ParkKing.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICarRepository carRepo;

        public HomeController(ICarRepository carRepo)
        {
            this.carRepo = carRepo;
        }

        public IActionResult Index()
        {
            // calc amount of spaces left
            var spaces = carRepo.BayAmount - carRepo.GetAll().Count();

            return View((AvailableSpace: spaces, UserHasCarParked: false ));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
