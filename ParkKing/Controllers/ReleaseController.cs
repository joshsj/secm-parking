using Microsoft.AspNetCore.Mvc;
using ParkKing.Data.CarRepository;
using ParkKing.Models;
using ParkKing.ViewModels;

namespace ParkKing.Controllers
{
    public class ReleaseController : Controller
    {
        private readonly ICarRepository carRepo;

        public ReleaseController(ICarRepository carRepo)
        {
            this.carRepo = carRepo;
        }

        public IActionResult Index() => View(new Car());

        [HttpPost]
        public IActionResult Index(Car car)
        {
            var releaseResult = carRepo.Release(car);

            // check released
            if (releaseResult == ReleaseResult.Released)
            {
                return View("Message", new MessageViewModel
                {
                    IsSuccessMessage = true,
                    Message = "Vehicle released."
                });
            }

            // check for errors
            if (releaseResult == ReleaseResult.BadBayNumber)
            {
                ModelState.AddModelError(nameof(Car.BayNumber), "Bay number not in use.");
            }
            
            if (releaseResult == ReleaseResult.BadPassword)
            {
                ModelState.AddModelError(nameof(Car.Password), "Password incorrect.");
            }

            return View(car);
        }
    }
}