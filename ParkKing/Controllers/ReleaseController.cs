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
            var releaseResult = carRepo.Release(car, Authentication.Password);

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

        public IActionResult ForgottenPassword() => View();

        [HttpPost]
        public IActionResult ForgottenPassword(Car car)
        {
            // get otp from repo
            switch (carRepo.GenerateOtp(car.BayNumber))
            {
                case GenerateOtpResult.BadBayNumber:
                    ModelState.AddModelError(nameof(Car.BayNumber), "Bay number not in use.");
                    return View(car);

                case GenerateOtpResult.NoPhoneNumber:
                    return View("Message", new MessageViewModel
                    {
                        IsSuccessMessage = false,
                        Message = "A phone number was not registered with this vehicle. Please contact Park-King recovery."
                    });

                case GenerateOtpResult.Generated:
                    return RedirectToAction(nameof(Otp), car);
                    
                default:
                    return View("Message", new MessageViewModel
                    {
                        IsSuccessMessage = false,
                        Message = "An internal error occured. Sorry."
                    });
            }
        }

        public IActionResult Otp(Car car)
            => View(car);

        [HttpPost("Otp")]
        public IActionResult OtpPost(Car car)
        {
            // try release with otp
            switch (carRepo.Release(car, Authentication.Otp))
            {
                case ReleaseResult.BadOtp:
                    return View("Message", new MessageViewModel
                    {
                        IsSuccessMessage = false,
                        Message = $"One-time verifation failed. Retry within {carRepo.OtpTimeout.TotalMinutes} minutes."
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