using Microsoft.AspNetCore.Mvc;
using ParkKing.Data.VehicleRepository;
using ParkKing.Models;
using ParkKing.ViewModels;

namespace ParkKing.Controllers
{
    public class ReleaseController : Controller
    {
        private readonly IVehicleRepository vehicleRepo;

        public ReleaseController(IVehicleRepository vehicleRepo)
        {
            this.vehicleRepo = vehicleRepo;
        }

        public IActionResult Index() => View(new Vehicle());

        [HttpPost]
        public IActionResult Index(Vehicle vehicle)
        {
            var releaseResult = vehicleRepo.Release(vehicle, Authentication.Password);

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
                ModelState.AddModelError(nameof(Vehicle.BayNumber), "Bay number not in use.");
            }

            if (releaseResult == ReleaseResult.BadPassword)
            {
                ModelState.AddModelError(nameof(Vehicle.Password), "Password incorrect.");
            }

            return View(vehicle);
        }

        public IActionResult ForgottenPassword() => View();

        [HttpPost]
        public IActionResult ForgottenPassword(Vehicle vehicle)
        {
            // get otp from repo
            switch (vehicleRepo.GenerateOtp(vehicle.BayNumber))
            {
                case GenerateOtpResult.BadBayNumber:
                    ModelState.AddModelError(nameof(Vehicle.BayNumber), "Bay number not in use.");
                    return View(vehicle);

                case GenerateOtpResult.NoPhoneNumber:
                    return View("Message", new MessageViewModel
                    {
                        IsSuccessMessage = false,
                        Message = "A phone number was not registered with this vehicle. Please contact Park-King recovery."
                    });

                case GenerateOtpResult.Generated:
                    return RedirectToAction(nameof(Otp), vehicle);
                    
                default:
                    return View("Message", new MessageViewModel
                    {
                        IsSuccessMessage = false,
                        Message = "An internal error occured. Sorry."
                    });
            }
        }

        public IActionResult Otp(Vehicle vehicle)
            => View(vehicle);

        [HttpPost("Otp")]
        public IActionResult OtpPost(Vehicle vehicle)
        {
            // try release with otp
            switch (vehicleRepo.Release(vehicle, Authentication.Otp))
            {
                case ReleaseResult.BadOtp:
                    return View("Message", new MessageViewModel
                    {
                        IsSuccessMessage = false,
                        Message = $"One-time verifation failed. Retry within {vehicleRepo.OtpTimeout.TotalSeconds} seconds."
                    });

                case ReleaseResult.Released:
                    return View("Message", new MessageViewModel
                    {
                        IsSuccessMessage = true,
                        Message = "Vehicle released!"
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