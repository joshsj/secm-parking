using Microsoft.AspNetCore.Mvc;
using ParkKing.Data.CarRepository;
using ParkKing.Models;
using ParkKing.ViewModels;

namespace ParkKing.Controllers
{
    public class SecureController : Controller
    {
        private readonly ICarRepository carRepo;

        public SecureController(ICarRepository carRepo)
        {
            this.carRepo = carRepo;
        }

        public IActionResult Index()
            => View(new Car());

        [HttpPost]
        public IActionResult Index(Car car)
        {
            // check bay is available
            if (!carRepo.IsBayAvailable(car.BayNumber))
            {
                // add error to model
                ModelState.AddModelError(nameof(Car.BayNumber), "Bay is occupied by another vehicle.");
                return View(car);
            }

            return RedirectToAction(nameof(Details), car);
        }

        public IActionResult Details(Car car)
            => View(car);

        [HttpPost]
        public IActionResult Finalise(Car car)
        {
            switch (carRepo.Secure(car))
            {
                case SecureResult.BadBayNumber:
                    return View("Message", new MessageViewModel
                    {
                        IsSuccessMessage = false,
                        Message = "There was a problem securing your vehicle."
                    });

                case SecureResult.Secured:
                    return View("Message", new MessageViewModel
                    {
                        IsSuccessMessage = true,
                        Message = "Vehicle secured!"
                    });

                default:
                    return View("Message", new MessageViewModel
                    {
                        IsSuccessMessage = false,
                        Message = "An internal error occured. Sorry."
                    });
            }
        }
    }
}