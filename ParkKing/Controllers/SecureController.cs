using Microsoft.AspNetCore.Mvc;
using ParkKing.Data.VehicleRepository;
using ParkKing.Models;
using ParkKing.ViewModels;

namespace ParkKing.Controllers
{
    public class SecureController : Controller
    {
        private readonly IVehicleRepository vehicleRepo;

        public SecureController(IVehicleRepository vehicleRepo)
        {
            this.vehicleRepo = vehicleRepo;
        }

        public IActionResult Index()
            => View(new Vehicle());

        [HttpPost]
        public IActionResult Index(Vehicle vehicle)
        {
            // check bay is available
            if (!vehicleRepo.IsBayAvailable(vehicle.BayNumber))
            {
                // add error to model
                ModelState.AddModelError(nameof(Vehicle.BayNumber), "Bay unavailable.");
                return View(vehicle);
            }

            return View("Details", vehicle);
        }

        [HttpPost]
        public IActionResult Finalise(Vehicle vehicle)
        {
            switch (vehicleRepo.Secure(vehicle))
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